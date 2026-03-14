namespace orchid_backend_net.Application.Seedling.Dto
{
    /// <summary>
    /// <ul>
    /// <li>Kết quả so sánh tỷ lệ thành công để Researcher chọn tổ hợp lai tốt nhất.</li>
    /// </ul>
    /// </summary>
    public class HybridSuccessRateDto
    {
        public string? SeedlingParentId { get; set; }
        public string SeedlingParentName { get; set; } = default!;
        public int MethodId { get; set; }
        public string MethodName { get; set; } = default!;
        public int TotalExperiments { get; set; }
        public int CompletedExperiments { get; set; }
        public double SuccessRate { get; set; }         // % hoàn thành
        public double AverageSurvivalRate { get; set; } // % mẫu sống trung bình
    }
}
