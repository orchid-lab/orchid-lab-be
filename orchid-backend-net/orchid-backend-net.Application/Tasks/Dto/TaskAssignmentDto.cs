using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Tasks.Dto
{
    public class TaskAssignmentDto : IMapFrom<TaskAssignment>
    {
        public string TaskId { get; set; }
        public string TechnicianName { get; set; }
        public string? SampleName { get; set; }
        public bool IsForWholeExperimentLog { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TaskAssignment, TaskAssignmentDto>()
                .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.Technician.Name))
                .ForMember(dest => dest.SampleName, opt => opt.MapFrom(src => src.Sample != null ? src.Sample.Name : null));
        }
    }
}
