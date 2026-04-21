using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.MethodStageDefinition.Dto
{
    public class MethodStageDefinitionDto : IMapFrom<Domain.Entities.MethodStageDefinition>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.MethodStageDefinition, MethodStageDefinitionDto>();
        }
    }
}
