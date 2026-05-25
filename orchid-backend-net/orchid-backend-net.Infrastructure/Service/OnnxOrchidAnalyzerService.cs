using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;

namespace orchid_backend_net.Infrastructure.Service
{
    public class OnnxOrchidAnalyzerService : IOrchidAnalyzerService, IDisposable
    {
        private readonly InferenceSession? _stageSession;
        private readonly InferenceSession? _diseaseSession;
        private readonly ILogger<OnnxOrchidAnalyzerService> _logger;
        private readonly ConcurrentDictionary<string, OrchidAnalysisResult> _cache;
        private readonly SemaphoreSlim _analyzeSemaphore = new(1, 1);
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed;
        private readonly int _inputWidth;
        private readonly int _inputHeight;
        private readonly string _inputName;
        private const int CacheCapacity = 200;

        // Fallback khi DB lỗi
        private static readonly string[] DiseaseClassesFallback =
        {
            "Anthracnose", "BacterialWilt", "Blackrot", "Brownspots",
            "MoldBacterial", "MoldFungus", "SoftRot", "StemRot",
            "WitheredYellowRoot", "Healthy", "Oxidation", "Virus"
        };

        private string[] _diseaseClasses;

        private static readonly string[] StageClasses =
        {
            "coppice",
            "tissue",
            "tree"
        };

        public OnnxOrchidAnalyzerService(
            ILogger<OnnxOrchidAnalyzerService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _cache = new ConcurrentDictionary<string, OrchidAnalysisResult>();
            _diseaseClasses = DiseaseClassesFallback;

            _logger.LogInformation("🚀 Loading ONNX models...");
            var loadTimer = Stopwatch.StartNew();

            try
            {
                var sessionOptions = new SessionOptions
                {
                    ExecutionMode = ExecutionMode.ORT_SEQUENTIAL,
                    GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,
                    InterOpNumThreads = 2,
                    IntraOpNumThreads = 2
                };

                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var stageModelPath = Path.Combine(baseDir, "Models", "stage_model.onnx");
                var diseaseModelPath = Path.Combine(baseDir, "Models", "disease_model.onnx");

                if (!File.Exists(stageModelPath))
                    throw new FileNotFoundException($"Stage model not found: {stageModelPath}");
                if (!File.Exists(diseaseModelPath))
                    throw new FileNotFoundException($"Disease model not found: {diseaseModelPath}");

                _logger.LogInformation("Loading stage model: {Path}", stageModelPath);
                _stageSession = new InferenceSession(stageModelPath, sessionOptions);

                _logger.LogInformation("Loading disease model: {Path}", diseaseModelPath);
                _diseaseSession = new InferenceSession(diseaseModelPath, sessionOptions);

                var stageInput = _stageSession.InputMetadata.First();
                _inputName = stageInput.Key;
                var dims = stageInput.Value.Dimensions;
                _inputHeight = dims[2];
                _inputWidth = dims[3];

                _logger.LogInformation("✅ Model input: {Name} [{H}x{W}]", _inputName, _inputHeight, _inputWidth);

                loadTimer.Stop();
                _logger.LogInformation("✅ ONNX models loaded in {Time}ms", loadTimer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to load ONNX models");
            }
        }

        private async Task RefreshDiseaseClassesAsync(CancellationToken ct)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IDiseaseRepository>();

                var diseases = await repo.FindAllAsync(
                    d => d.IsActive && d.OnnxClassName != null,
                    ct);

                var classes = diseases
                    .Where(d => !string.IsNullOrEmpty(d.OnnxClassName))
                    .OrderBy(d => d.ID)
                    .Select(d => d.OnnxClassName!)
                    .ToArray();

                if (classes.Length > 0)
                {
                    _diseaseClasses = classes;
                    _logger.LogInformation("✅ Loaded {Count} disease classes from DB: {Classes}",
                        classes.Length, string.Join(", ", classes));
                }
                else
                {
                    _diseaseClasses = DiseaseClassesFallback;
                    _logger.LogWarning("⚠️ No active disease classes found in DB, using fallback");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "⚠️ Failed to load disease classes from DB, using fallback");
                _diseaseClasses = DiseaseClassesFallback;
            }
        }

        public async Task<OrchidAnalysisResult> AnalyzeAsync(byte[] imageBytes, CancellationToken cancellationToken)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            // Load disease classes từ DB — đảm bảo luôn cập nhật khi CRUD
            await RefreshDiseaseClassesAsync(cancellationToken);

            var requestTimer = Stopwatch.StartNew();
            var imageHash = ComputeImageHash(imageBytes);

            if (_cache.TryGetValue(imageHash, out var cachedResult))
            {
                _logger.LogInformation("✓ Cache HIT: {Hash} ({Time}ms)",
                    imageHash[..12], requestTimer.ElapsedMilliseconds);
                return cachedResult;
            }

            await _analyzeSemaphore.WaitAsync(cancellationToken);
            try
            {
                if (_cache.TryGetValue(imageHash, out cachedResult))
                {
                    _logger.LogInformation("✓ Cache HIT after gate: {Hash} ({Time}ms)",
                        imageHash[..12], requestTimer.ElapsedMilliseconds);
                    return cachedResult;
                }

                using var image = Image.Load<Rgb24>(imageBytes);
                var inputTensor = PreprocessImage(image);

                var stageResult = RunInference(_stageSession!, inputTensor, StageClasses, "Stage");
                var diseaseResult = RunInference(_diseaseSession!, inputTensor, _diseaseClasses, "Disease");

                var result = new OrchidAnalysisResult
                {
                    Stage = stageResult.PredictedClass,
                    Disease = new OrchidAnalysisDiseaseResult
                    {
                        Predict = diseaseResult.PredictedClass,
                        Probability = diseaseResult.Probabilities
                    }
                };

                CacheResult(imageHash, result);

                requestTimer.Stop();
                _logger.LogInformation(
                    "✓ Analysis completed in {Time}ms (Stage: {Stage}, Disease: {Disease})",
                    requestTimer.ElapsedMilliseconds, result.Stage, result.Disease.Predict);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ONNX analysis failed");
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                _analyzeSemaphore.Release();
            }
        }

        private Tensor<float> PreprocessImage(Image<Rgb24> image)
        {
            if (image.Width != _inputWidth || image.Height != _inputHeight)
            {
                image.Mutate(x => x.Resize(_inputWidth, _inputHeight));
            }

            var planeSize = _inputWidth * _inputHeight;
            var buffer = new float[3 * planeSize];

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < _inputHeight; y++)
                {
                    var row = accessor.GetRowSpan(y);
                    for (int x = 0; x < _inputWidth; x++)
                    {
                        var pixel = row[x];
                        var hw = y * _inputWidth + x;

                        buffer[0 * planeSize + hw] = pixel.R / 255f;
                        buffer[1 * planeSize + hw] = pixel.G / 255f;
                        buffer[2 * planeSize + hw] = pixel.B / 255f;
                    }
                }
            });

            return new DenseTensor<float>(buffer, new[] { 1, 3, _inputHeight, _inputWidth });
        }

        private InferenceOutput RunInference(
            InferenceSession session,
            Tensor<float> inputTensor,
            string[] classNames,
            string modelType)
        {
            var inferenceTimer = Stopwatch.StartNew();

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            using var results = session.Run(inputs);

            var outputTensor = results.FirstOrDefault()?.AsEnumerable<float>().ToArray();

            _logger.LogInformation("{ModelType} output length: {Length}", modelType, outputTensor?.Length ?? 0);

            if (outputTensor == null || outputTensor.Length == 0)
                throw new InvalidOperationException($"{modelType} model produced no output");

            var probabilities = outputTensor;

            var count = Math.Min(probabilities.Length, classNames.Length);
            var probDict = new Dictionary<string, float>(count);

            var maxIdx = 0;
            var maxProb = probabilities[0];

            for (int i = 1; i < count; i++)
            {
                if (probabilities[i] > maxProb)
                {
                    maxProb = probabilities[i];
                    maxIdx = i;
                }
            }

            for (int i = 0; i < count; i++)
            {
                probDict[classNames[i]] = probabilities[i];
            }

            inferenceTimer.Stop();
            _logger.LogDebug("{ModelType} inference: {Time}ms", modelType, inferenceTimer.ElapsedMilliseconds);

            var predictedClass = classNames[maxIdx];
            if (modelType.Equals("Stage", StringComparison.OrdinalIgnoreCase))
                predictedClass = char.ToUpperInvariant(predictedClass[0]) + predictedClass[1..].ToLowerInvariant();

            return new InferenceOutput
            {
                PredictedClass = predictedClass,
                Probabilities = probDict
            };
        }
        private static string ComputeImageHash(byte[] imageBytes)
        {
            using var sha256 = SHA256.Create();
            var sampleSize = Math.Min(8192, imageBytes.Length);
            var hash = sha256.ComputeHash(imageBytes, 0, sampleSize);
            return Convert.ToHexString(hash)[..32];
        }

        private void CacheResult(string hash, OrchidAnalysisResult result)
        {
            _cache.TryAdd(hash, result);

            if (_cache.Count > CacheCapacity)
            {
                var toRemove = _cache.Keys.Take(_cache.Count - CacheCapacity).ToList();
                foreach (var key in toRemove)
                    _cache.TryRemove(key, out _);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _stageSession?.Dispose();
                _diseaseSession?.Dispose();
                _analyzeSemaphore.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal class InferenceOutput
    {
        public string PredictedClass { get; set; } = string.Empty;
        public Dictionary<string, float> Probabilities { get; set; } = new();
    }
}