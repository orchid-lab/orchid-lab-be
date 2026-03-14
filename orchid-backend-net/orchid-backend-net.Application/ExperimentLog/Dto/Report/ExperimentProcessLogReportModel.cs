namespace orchid_backend_net.Application.ExperimentLog.Dto.Report
{
    /// <summary>
    /// <ul>
    /// <li>Model cho Report 1 — Process Log.</li>
    /// <li>Chứa data analytics: timeline, trạng thái mẫu theo giai đoạn, AI results, sự cố bệnh.</li>
    /// </ul>
    /// </summary>
    public class ExperimentProcessLogReportModel
    {
        // Header
        public string ExperimentName { get; set; } = default!;
        public string MethodName { get; set; } = default!;
        public string SeedlingLocalName { get; set; } = default!;
        public string SeedlingScientificName { get; set; } = default!;
        public string ResearcherName { get; set; } = default!;
        public string TechnicianName { get; set; } = default!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string GeneratedAt { get; set; } = default!;

        // Section 1: Tổng quan mẫu vật
        public int TotalSamples { get; set; }
        public int ExpectedSamples { get; set; }
        public int AliveSamples { get; set; }
        public int InfectedSamples { get; set; }
        public double SurvivalRate { get; set; }

        // Section 2: Phân bố theo giai đoạn sinh học
        public List<SampleStageProgressItem> StageProgress { get; set; } = new();

        // Section 3: Timeline giai đoạn method (thực tế vs dự kiến)
        public List<MethodStageTimelineItem> MethodStageTimeline { get; set; } = new();

        // Section 4: AI analysis results
        public List<AIAnalysisItem> AIAnalysisResults { get; set; } = new();

        // Section 5: Disease incidents
        public List<DiseaseIncidentReportItem> DiseaseIncidents { get; set; } = new();

        // Section 6: Task summary
        public int TotalTasks { get; set; }
        public int TasksCompletedOnTime { get; set; }
        public int TasksCompletedLate { get; set; }
    }

    public class SampleStageProgressItem
    {
        public string StageName { get; set; } = default!;
        public int SampleCount { get; set; }
        public double Percentage { get; set; }
    }

    public class MethodStageTimelineItem
    {
        public int StageOrder { get; set; }
        public string StageName { get; set; } = default!;
        public int PlannedDays { get; set; }
        public int? ActualDays { get; set; }
        public string Status { get; set; } = default!;
    }

    public class AIAnalysisItem
    {
        public string SampleName { get; set; } = default!;
        public string StageName { get; set; } = default!;
        public string DetectedDisease { get; set; } = default!;
        public double Confidence { get; set; }
        public string IncidentStatus { get; set; } = default!;
        public string AnalyzedAt { get; set; } = default!;
    }

    public class DiseaseIncidentReportItem
    {
        public string SampleName { get; set; } = default!;
        public string DiseaseName { get; set; } = default!;
        public double AIConfidence { get; set; }
        public string IncidentStatus { get; set; } = default!;
        public string? ReviewNote { get; set; }
        public List<string> Actions { get; set; } = new();
    }
}
