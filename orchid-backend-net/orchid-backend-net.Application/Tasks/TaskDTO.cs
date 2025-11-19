using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.TaskAssign;
using orchid_backend_net.Application.TaskAttribute;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Tasks
{
    public class TaskDTO : IMapFrom<Domain.Entities.Tasks>
    {
        public string ID { get; set; }
        public string ExperimentLogName { get; set; }
        public string SampleName { get; set; }
        public string Researcher { get; set; }
        public List<TaskAssignDTO> AssignDTOs { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TaskAttributeDTO> AttributeDTOs { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public DateTime Create_at { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public string Url { get; set; }
        public string ReportInformation { get; set; }
        public bool IsDaily { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tasks, TaskDTO>()
                .ForMember(dest => dest.AssignDTOs, opt => opt.MapFrom(src => src.Assigns))
                .ForMember(dest => dest.AttributeDTOs, opt => opt.MapFrom(src => src.Attributes))
                .ForMember(dest => dest.ExperimentLogName, opt => opt.MapFrom(src => 
                    src.Linkeds.FirstOrDefault(linked => linked.TaskID.Equals(src.ID)).ExperimentLog.Name))
                .ForMember(dest => dest.SampleName, opt => opt.MapFrom(src => 
                    src.Linkeds.FirstOrDefault(linked => linked.TaskID.Equals(src.ID)).Sample.Name));
        }
    }
}
