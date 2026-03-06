using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.Sample.Dto.SampleStageDefinition
{
    public class SampleStageDefinitionDto : IMapFrom<Domain.Entities.SampleStageDefinition>
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public int? MinDurationDays { get; set; }
        public int? MaxDurationDays { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SampleStageDefinition, SampleStageDefinitionDto>();
        }
    }
}
