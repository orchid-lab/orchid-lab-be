using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckList;

namespace orchid_backend_net.Application.Tasks.Dto.Task
{
    public class TaskDetailDto : IMapFrom<Domain.Entities.Tasks>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public List<TaskAttributesDto> TaskAttributes { get; set; }
        public TaskAssignmentsDto TaskAssignments { get; set; }
        public TaskCheckListDto? TaskCheckList { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tasks, TaskDetailDto>()
                .ForMember(dest => dest.TaskAttributes, opt => opt.MapFrom(src => src.TaskAttributes))
                .ForMember(dest => dest.TaskAssignments, opt => opt.MapFrom(src => src.TaskAssignment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToDisplayText()))
                .ForMember(dest => dest.TaskCheckList, opt => opt.MapFrom(src => src.CheckList));
        }
    }
}