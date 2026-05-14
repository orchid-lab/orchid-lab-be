using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;
using System.Text.Json;

namespace orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult
{
    public class AnalyticResultDto : IMapFrom<Domain.Entities.AnalyticResults>
    {
        public required string Id { get; set; }
        public required Dictionary<string, decimal> Predictions { get; set; }
        public required string TopDisease { get; set; }
        public required decimal Confidence { get; set; }
        public DateTime AnalyzedAt { get; set; }

        // Helper method — không dùng optional arguments
        private static Dictionary<string, decimal> DeserializePredictions(string? json)
        {
            if (string.IsNullOrEmpty(json)) return new Dictionary<string, decimal>();
            return JsonSerializer.Deserialize<Dictionary<string, decimal>>(json)
                   ?? new Dictionary<string, decimal>();
        }

        public static AnalyticResultDto Create(AnalyticResults entity)
        {
            return new AnalyticResultDto
            {
                Id = entity.ID.ToString(),
                Predictions = DeserializePredictions(entity.PredictionsJson),
                TopDisease = entity.TopDisease,
                Confidence = entity.Confidence,
                AnalyzedAt = entity.AnalyzedAt
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AnalyticResults, AnalyticResultDto>()
                .ForMember(d => d.Predictions,
                    opt => opt.MapFrom(s => DeserializePredictions(s.PredictionsJson)));
        }
    }
}