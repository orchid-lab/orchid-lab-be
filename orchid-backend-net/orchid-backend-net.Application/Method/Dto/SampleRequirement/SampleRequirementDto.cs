using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.SampleRequirement
{
    public class SampleRequirementDto : IMapFrom<SamplesRequirements>
    {
        public string? CharacteristicCode { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal MinValue { get; set; }
        public required decimal MaxValue { get; set; }
        public required decimal ExpectedValue { get; set; }
        public required string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SamplesRequirements, SampleRequirementDto>();
        }
    }
}
