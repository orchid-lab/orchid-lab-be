using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;

namespace orchid_backend_net.Infrastructure.Service
{
    /// <summary>
    /// ONNX-based orchid analyzer service.
    /// Runs AI inference directly in .NET process (no Python API required).
    /// Implements in-memory caching for fast repeated analysis.
    /// </summary>
    public class OnnxOrchidAnalyzerService : IOrchidAnalyzerService, IDisposable
    {
        //Declare as nullable to allow null until constructor assigns them
        private readonly InferenceSession? _stageSession;
        private readonly InferenceSession? _diseaseSession;
        private readonly ILogger<OnnxOrchidAnalyzerService> _logger;
        private readonly ConcurrentDictionary<string, OrchidAnalysisResult> _cache;
        private readonly SemaphoreSlim _analyzeSemaphore = new(1, 1);

        //Dispose pattern field
        private bool _disposed;

        private const int ImageSize = 192;
        private const int CacheCapacity = 200;

        /// <summary>
        /// Stage classes from ONNX model (3 stages)
        /// </summary>
        private static readonly string[] StageClasses =
        {
            "Coppice",  // Giai đoạn chồi non
            "Tissue",   // Giai đoạn mô nuôi cấy
            "Tree"      // Giai đoạn cây trưởng thành
        };

        /// <summary>
        /// Disease classes from ONNX model (12 diseases)
        /// </summary>
        private static readonly string[] DiseaseClasses =
        {
            "Anthracnose",
            "BacterialWilt",
            "Blackrot",
            "Brownspots",
            "MoldBacterial",
            "MoldFungus",
            "SoftRot",
            "StemRot",
            "WitheredYellowRoot",
            "Healthy",
            "Oxidation",
            "Virus"
        };

        public OnnxOrchidAnalyzerService(ILogger<OnnxOrchidAnalyzerService> logger)
        {
            _logger = logger;
            _cache = new ConcurrentDictionary<string, OrchidAnalysisResult>();

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

                loadTimer.Stop();
                _logger.LogInformation("✅ ONNX models loaded in {Time}ms", loadTimer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to load ONNX models");
                throw new ArgumentException(ex.Message);
            }
        }

        /// <inheritdoc />
        public async Task<OrchidAnalysisResult> AnalyzeAsync(byte[] imageBytes, CancellationToken cancellationToken)
        {
            // Guard against disposed state
            ObjectDisposedException.ThrowIf(_disposed, this);

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

                // Optimize: Run inferences sequentially instead of parallel Task.Run
                // On 4GB VPS: thread pool overhead > parallelization benefit for ~5µs tasks
                // Sequential execution eliminates context switching and contention
                var stageResult = RunInference(_stageSession!, inputTensor, StageClasses, "Stage");
                var diseaseResult = RunInference(_diseaseSession!, inputTensor, DiseaseClasses, "Disease");

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

        /// <summary>
        /// Preprocess image to ONNX input tensor [1, 3, H, W]
        /// </summary>
        private static Tensor<float> PreprocessImage(Image<Rgb24> image)
        {
            image.Mutate(x => x.Resize(ImageSize, ImageSize));

            // NCHW flat layout: index = channel * (H * W) + y * W + x
            // batch = 1, channels = 3 (R/G/B), H = W = ImageSize
            var buffer = new float[3 * ImageSize * ImageSize];

            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < ImageSize; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < ImageSize; x++)
                    {
                        var pixel = pixelRow[x];
                        var hw = y * ImageSize + x;                      // spatial offset
                        buffer[0 * ImageSize * ImageSize + hw] = pixel.R / 255f;  // R channel
                        buffer[1 * ImageSize * ImageSize + hw] = pixel.G / 255f;  // G channel
                        buffer[2 * ImageSize * ImageSize + hw] = pixel.B / 255f;  // B channel
                    }
                }
            });

            // Wrap flat buffer in a DenseTensor with shape [1, 3, H, W]
            return new DenseTensor<float>(buffer, new[] { 1, 3, ImageSize, ImageSize });
        }

        /// <summary>
        /// Run ONNX inference and return class probabilities
        /// </summary>
        private InferenceOutput RunInference(
            InferenceSession session,
            Tensor<float> inputTensor,
            string[] classNames,
            string modelType)
        {
            var inferenceTimer = Stopwatch.StartNew();

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("images", inputTensor)
            };

            using var results = session.Run(inputs);

            var outputTensor = results.FirstOrDefault()?.AsEnumerable<float>().ToArray();

            if (outputTensor == null || outputTensor.Length == 0)
                throw new InvalidOperationException($"{modelType} model produced no output");

            var probabilities = Softmax(outputTensor);

            var maxIdx = 0;
            var maxProb = probabilities[0];
            for (int i = 1; i < probabilities.Length; i++)
            {
                if (probabilities[i] > maxProb)
                {
                    maxProb = probabilities[i];
                    maxIdx = i;
                }
            }

            // Build dict safely with bounds check to prevent duplicate keys
            var count = Math.Min(probabilities.Length, classNames.Length);
            var probDict = new Dictionary<string, float>(count);
            for (int i = 0; i < count; i++)
            {
                probDict[classNames[i]] = probabilities[i];
            }

            inferenceTimer.Stop();
            _logger.LogDebug("{ModelType} inference: {Time}ms", modelType, inferenceTimer.ElapsedMilliseconds);

            return new InferenceOutput
            {
                PredictedClass = classNames[maxIdx],
                Probabilities = probDict
            };
        }

        /// <summary>
        /// Softmax activation function
        /// </summary>
        private static float[] Softmax(float[] logits)
        {
            var length = logits.Length;
            var max = logits[0];
            for (int i = 1; i < length; i++)
            {
                if (logits[i] > max)
                {
                    max = logits[i];
                }
            }

            var probabilities = new float[length];
            var sum = 0f;
            for (int i = 0; i < length; i++)
            {
                var value = MathF.Exp(logits[i] - max);
                probabilities[i] = value;
                sum += value;
            }

            if (sum <= 0f)
            {
                return probabilities;
            }

            var invSum = 1f / sum;
            for (int i = 0; i < length; i++)
            {
                probabilities[i] *= invSum;
            }

            return probabilities;
        }

        /// <summary>
        /// Compute fast hash for caching (first 8KB only)
        /// </summary>
        private static string ComputeImageHash(byte[] imageBytes)
        {
            using var sha256 = SHA256.Create();
            var sampleSize = Math.Min(8192, imageBytes.Length);
            var hash = sha256.ComputeHash(imageBytes, 0, sampleSize);
            return Convert.ToHexString(hash)[..32];
        }

        /// <summary>
        /// Cache result with simple LRU eviction
        /// </summary>
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

        //Proper IDisposable pattern
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

    /// <summary>
    /// Internal inference output
    /// </summary>
    internal class InferenceOutput
    {
        public string PredictedClass { get; set; } = string.Empty;
        public Dictionary<string, float> Probabilities { get; set; } = new();
    }
}