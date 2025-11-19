using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.TaskAssign
{
    public class TaskAssignDTO : IMapFrom<TasksAssign>
    {
        public string Id { get; set; }
        public string TechnicianName { get; set; }
        public bool Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TasksAssign, TaskAssignDTO>()
                .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.Technician.Name))
                .ReverseMap();
        }
    }
}
