using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;

namespace orchid_backend_net.Application.Tasks.Dto
{
    public class TaskDto : IMapFrom<Domain.Entities.Tasks>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? StageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public List<TaskAttributesDto> TaskAttributes { get; set; }
        public List<TaskAssignmentsDto> TaskAssignments { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tasks, TaskDto>()
                .ForMember(dest => dest.TaskAttributes, opt => opt.MapFrom(src => src.TaskAttributes))
                .ForMember(dest => dest.TaskAssignments, opt => opt.MapFrom(src => src.TaskAssignments))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToDisplayText()));
        }
    }
}