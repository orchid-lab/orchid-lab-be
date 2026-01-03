using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto
{
    public class TaskAttributesDto : IMapFrom<TaskAttributes>
    {
        public string ChemicalName { get; set; }
        public string MaterialName { get; set; }
        public string Unit { get; set; }
        public decimal Value { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskAttributes, TaskAttributesDto>()
                .ForMember(d => d.ChemicalName, opt => opt.MapFrom(s => s.Chemicals.Name))
                .ForMember(d => d.MaterialName, opt => opt.MapFrom(s => s.Materials.Name));
        }
    }
}