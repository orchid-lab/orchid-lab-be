using System;
using System.Collections.Generic;

namespace orchid_backend_net.Application.ExperimentLog.Dto.Report
{
    /// <summary>
    /// <ul>
    /// <li>Model cho Report 2 — Summary Report.</li>
    /// <li>Dùng để nộp hội đồng hoặc lưu hồ sơ lab. Có đủ Objective → Conclusion → Recommendations.</li>
    /// </ul>
    /// </summary>
    public class ExperimentSummaryReportModel
    {
        // Cover
        public string ExperimentName { get; set; } = default!;
        public string MethodName { get; set; } = default!;
        public string SeedlingLocalName { get; set; } = default!;
        public string SeedlingScientificName { get; set; } = default!;
        public string ResearcherName { get; set; } = default!;
        public string TechnicianName { get; set; } = default!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string GeneratedAt { get; set; } = default!;

        // Mục 1: Mục tiêu
        public string? Objective { get; set; }

        // Mục 2: Tóm tắt quy trình (timeline)
        public List<MethodStageTimelineItem> MethodStageTimeline { get; set; } = new();

        // Mục 3: Kết quả mẫu vật
        public int TotalSamples { get; set; }
        public int ExpectedSamples { get; set; }
        public int AliveSamples { get; set; }
        public int InfectedSamples { get; set; }
        public double SurvivalRate { get; set; }
        public List<SampleStageProgressItem> FinalStageDistribution { get; set; } = new();

        // Mục 4: Kết quả AI phân tích tóm tắt
        public int TotalAIScans { get; set; }
        public int DiseasesDetected { get; set; }
        public int DiseasesConfirmedByHuman { get; set; }
        public int DiseasesDismissedByHuman { get; set; }
        public List<string> TopDiseasesFound { get; set; } = new();

        // Mục 5: Kết luận & Đề xuất
        public string? Conclusion { get; set; }
        public string? Issues { get; set; }
        public string? Recommendations { get; set; }

        // Mục 6: Xác nhận
        public string? ResearcherSignature { get; set; }
        public string CompletedDate { get; set; } = default!;
    }
}
