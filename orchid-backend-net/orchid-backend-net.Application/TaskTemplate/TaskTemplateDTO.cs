using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.TaskTemplate
{
    public class TaskTemplateDTO : IMapFrom<TaskTemplates>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string StageID {  get; set; }
        public string StageName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public List<TaskTemplateDetailsDTO> Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskTemplates, TaskTemplateDTO>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.TemplateDetails))
                .ForMember(dest => dest.StageName, opt => opt.MapFrom(src => src.Stage.Name))
                .ReverseMap();
        }
    }
}
