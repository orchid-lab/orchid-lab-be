using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.SampleRequirementDefinition.Dto;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Sample.Dto.StageRequirementDefinitionDto
{
    public class StageRequirementDefinitionDto : IMapFrom<StageRequirementDefinition>
    {
        public required string Id { get; set; }
        public SampleRequirementDefinitionDto SampleRequirementDefinitionDto { get; set; } = default!;
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public required decimal ExpectedValue { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<StageRequirementDefinition, StageRequirementDefinitionDto>();
        }
    }
}
