namespace orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog
{
    /// <summary>
    /// <ul>
    /// <li>DTO trả về cho dashboard analytics của Researcher.</li>
    /// </ul>
    /// </summary>
    public class ExperimentLogSummaryDto
    {
        public string ExperimentLogId { get; set; } = default!;
        public string ExperimentLogName { get; set; } = default!;

        // Tổng quan mẫu vật
        public int TotalSamples { get; set; }
        public int ExpectedSamples { get; set; }
        public int AliveSamples { get; set; }
        public int InfectedSamples { get; set; }
        public double SurvivalRate { get; set; }       // % so với tổng đã tạo
        public double ProgressRate { get; set; }        // % so với mục tiêu ban đầu

        // Phân bố theo giai đoạn
        public List<SampleStageDistributionDto> StageDistribution { get; set; } = new();

        // Monitoring
        public int TotalMonitoringLogs { get; set; }
        public int PendingApprovalLogs { get; set; }
        public int RejectedLogs { get; set; }
    }

    /// <summary>
    /// <ul>
    /// <li>Phân bố mẫu theo giai đoạn sinh học</li>
    /// </ul>
    /// </summary>
    public class SampleStageDistributionDto
    {
        public string StageName { get; set; } = default!;
        public int SampleCount { get; set; }
        public double Percentage { get; set; }
    }
}
