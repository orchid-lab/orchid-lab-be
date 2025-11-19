namespace orchid_backend_net.Domain.Entities
{
    public class OrchidAnalysisResult
    {
        public string Stage { get; set; } = string.Empty;
        public OrchidAnalysisDiseaseResult? Disease { get; set; }
    }
    public class OrchidAnalysisDiseaseResult
    {
        public string Predict { get; set; } = string.Empty;
        public Dictionary<string, float> Probability { get; set; } = new Dictionary<string, float>();
    }
}
