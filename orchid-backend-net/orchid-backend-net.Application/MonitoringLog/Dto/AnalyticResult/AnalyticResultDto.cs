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

        public static AnalyticResultDto Create(AnalyticResults entity)
        {
            var predictions = string.IsNullOrEmpty(entity.PredictionsJson)
                ? new Dictionary<string, decimal>()
                : JsonSerializer.Deserialize<Dictionary<string, decimal>>(entity.PredictionsJson)
                  ?? new Dictionary<string, decimal>();

            return new AnalyticResultDto
            {
                Id = entity.ID.ToString(),
                Predictions = predictions,
                TopDisease = entity.TopDisease,
                Confidence = entity.Confidence,
                AnalyzedAt = entity.AnalyzedAt
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AnalyticResults, AnalyticResultDto>();
        }
    }
}