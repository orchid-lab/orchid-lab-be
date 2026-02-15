using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem
{
    public class TaskCheckListItemDto : IMapFrom<Domain.Entities.TaskCheckListItem>
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }

        //Optional standard metadata
        public string? ExpectedUnit { get; set; }
        public decimal? ExpectedMinValue { get; set; }
        public decimal? ExpectedMaxValue { get; set; }

        //Researcher evalutaion result
        public TaskCheckListItemStatus Status { get; set; }

        public string? MeasurementUnit { get; set; }
        public decimal? MesuredValue { get; set; }
        public bool? IsPass { get; set; }
        public DateTime? Evaluated { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.TaskCheckListItem, TaskCheckListItemDto>();
        }
    }
}
