using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Service;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class OrchidAnalyzerService(IConfiguration configuration) : IOrchidAnalyzerService
    {
        public async Task<OrchidAnalysisResult> AnalyzeAsync(byte[] imageBytes)
        {
            var processedBytes = ResizeAndCompressingImage.ResizeAndCompressImages(imageBytes, 1024, 1024, 70);

            var pythonApiUrl = configuration["OrchidAnalyzer:PythonApiUrl"];
            if (string.IsNullOrEmpty(pythonApiUrl))
                throw new InvalidOperationException("OrchidAnalyzer:PythonApiUrl not configured");

            using var httpClient = new HttpClient();
            using var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(processedBytes), "file", "image.jpg" }
            };

            var response = await httpClient.PostAsync(pythonApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Python API failed: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OrchidAnalysisResult>(json);
            return result ?? throw new Exception("Invalid JSON result from Python.");
        }
    }
}
