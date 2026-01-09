using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.SampleRequirementDefinition.Dto
{
    public class SampleRequirementDefinitionDto : IMapFrom<SamplesRequirementsDefinition>
    {
        public required string  Id { get; set; }
        public string? CharacteristicCode { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SamplesRequirementsDefinition, SampleRequirementDefinitionDto>();
        }
    }
}
