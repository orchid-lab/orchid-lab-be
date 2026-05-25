using orchid_backend_net.Application.MonitoringLog.Dto.Disease;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult
{
    public class AnalyticResultAfterAnalysisDto
    {
        public string StageName { get; set; } = null!;
        public DiseaseDto Disease { get; set; } = null!;
        public AnalyticResultDto AnalyticResult { get; set; } = null!;
        public string RawTopDisease { get; set; } = string.Empty;
        public string SelectedDisease { get; set; } = string.Empty;
        public bool IsRawTopDiseaseActive { get; set; }
    }
}
