using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IOrchidAnalyzerService
    {
        Task<OrchidAnalysisResult> AnalyzeAsync(byte[] imageBytes);
    }
}
