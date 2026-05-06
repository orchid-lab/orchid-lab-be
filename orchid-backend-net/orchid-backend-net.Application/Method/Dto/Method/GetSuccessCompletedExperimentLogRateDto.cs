using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.Method
{
    public class GetSuccessCompletedExperimentLogRateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? TotalDurationDays { get; set; }
        public int? CompletedExperimentLog { get; set; }
        public int? FailedExperimentLog { get; set; }
        public int? SuccessRate { get; set; }
        public List<Seedlings>? Seedling { get; set; }
        public List<Domain.Entities.MethodStageDefinition>? MethodStages { get; set; }
    }
}
