using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class OrchidAnalyzerService : IOrchidAnalyzerService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };

        private readonly HttpClient _client;
        private readonly ILogger<OrchidAnalyzerService> _logger;
        public OrchidAnalyzerService(IConfiguration configuration, HttpClient client, ILogger<OrchidAnalyzerService> logger)
        {
            _client = client;
            _logger = logger;

            if(_client.BaseAddress == null)
            {
                var pythonApiUrl = configuration["OrchidAnalyzer:PythonApiUrl"];
                if (string.IsNullOrEmpty(pythonApiUrl))
                    throw new InvalidOperationException("OrchidAnalyzer:PythonApiUrl not configured");
                _client.BaseAddress = new Uri(pythonApiUrl);
            }

            _client.DefaultRequestVersion = System.Net.HttpVersion.Version20;
            _client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
        }
        public async Task<OrchidAnalysisResult> AnalyzeAsync(byte[] imageBytes, CancellationToken cancellationToken)
        {
            if(imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("Image bytes cannot be null or empty", nameof(imageBytes));

            //instrument latency check
            var sw = new Stopwatch();
            sw.Start();

            using var content = new MultipartFormDataContent();
            var bytesContent = new ByteArrayContent(imageBytes);
            bytesContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // controller compresses; adjust if PNG
            content.Add(bytesContent, "file", "image.jpg");

            using var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = content
            };
            request.Headers.Accept.ParseAdd("application/json");

            HttpResponseMessage response;
            try
            {
                response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (OperationCanceledException oce) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning(oce, "Orchid analyzer request timed out after {Elapsed} ms", sw.ElapsedMilliseconds);
                throw new TimeoutException("Analyzer request timed out.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("Python API failed with {StatusCode}. Error: {Error}", (int)response.StatusCode, error);
                throw new HttpRequestException($"Python API failed: {(int)response.StatusCode} - {error}");
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await System.Text.Json.JsonSerializer.DeserializeAsync<OrchidAnalysisResult>(stream, JsonOptions, cancellationToken);

            sw.Stop();
            _logger.LogDebug("Orchid analyzer completed in {Elapsed} ms", sw.ElapsedMilliseconds);

            return result ?? throw new InvalidOperationException("Invalid JSON result from Python.");
        }
    }
}
