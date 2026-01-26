using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem;

namespace orchid_backend_net.Application.Tasks.Dto.TaskCheckList
{
    public class TaskCheckListDto : IMapFrom<Domain.Entities.TaskCheckList>
    {
        public required string Id { get; set; }
        public List<TaskCheckListItemDto> CheckListItemDtos { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.TaskCheckList, TaskCheckListDto>()
                .ForMember(dest => dest.CheckListItemDtos,
                    opt => opt.MapFrom(
                        src => src.Items
                    )
                );
        }
    }
}
