using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.StageDefinitiones.Dto
{
    public class StageDefinitionDto : IMapFrom<MethodStageDefinition>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MethodStageDefinition, StageDefinitionDto>();
        }
    }
}
