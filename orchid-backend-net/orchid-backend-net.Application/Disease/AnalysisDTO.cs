using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Disease
{
    public class AnalysisDTO(string stage, DiseaseResultDTO disease)
    {
        public string Stage { get; set; } = stage;
        public DiseaseResultDTO? Disease { get; set; } = disease;
    }

    public class DiseaseResultDTO
    {
        public string Predict { get; set; } = string.Empty;
        public Dictionary<string, float> Probability { get; set; } = [];
    }
}
