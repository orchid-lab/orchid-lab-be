using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class AnalyticResults : BaseGuidEntity
    {
        public string PredictionsJson { get; set; } = "{}";
        public string TopDisease { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
        public string? AnalyzedBy { get; set; }  
        public virtual MonitoringLogs MonitoringLog { get; set; } = null!;
    }
}