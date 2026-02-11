using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Tasks.Dto.Task
{
    public class TaskDto : IMapFrom<Domain.Entities.Tasks>
    {
        public required string Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int? StageId { get; set; }
        public TaskTargetType TaskTargetType { get; set; }
        public string TargetId { get; set; } = default!;
        public string? ResearcherId { get; set; }
        public string? TechnicianId { get; set; }
        public Domain.Common.Enum.TaskStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tasks, TaskDto>()
                .ForMember(dest => dest.TechnicianId, opt => opt.MapFrom(src => src.TaskAssignment.TechnicianId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TaskTargetType, opt => opt.MapFrom(src => src.TaskAssignment.TargetType))
                .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.TaskAssignment.TargetId));
        }
    }
}
