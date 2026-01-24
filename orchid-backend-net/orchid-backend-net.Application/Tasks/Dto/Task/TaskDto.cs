using AutoMapper;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Mappings;

namespace orchid_backend_net.Application.Tasks.Dto.Task
{
    public class TaskDto : IMapFrom<Domain.Entities.Tasks>
    {
        public required string Id { get; set;  }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public string? TechnicianId { get; set; }
        public string Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tasks, TaskDto>()
                .ForMember(dest => dest.TechnicianId, opt => opt.MapFrom(src => src.TaskAssignment.TechnicianId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToDisplayText()));
        }
    }
}
