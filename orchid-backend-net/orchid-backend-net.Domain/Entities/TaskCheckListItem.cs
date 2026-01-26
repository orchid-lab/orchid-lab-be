using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskCheckListItem : BaseGuidEntity
    {
        public required string TaskCheckListId { get; set; }
        public virtual TaskCheckList TaskCheckList { get; set; } = null!;
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }

        //Optional standard metadata
        public string? ExpectedUnit { get; set; }
        public decimal? ExpectedMinValue { get; set; }
        public decimal? ExpectedMaxValue { get; set; }

        //Researcher evalutaion result
        public TaskCheckListItemStatus Status { get; set; } = TaskCheckListItemStatus.Pending;
        public string? MeasurementUnit { get; set; }
        public decimal? MesuredValue { get; set; }
        public bool? IsPass { get; set; }
        public DateTime? Evaluated { get; set; }

        //Researcher update item status after review
        public void SetResultByResearcher(TaskCheckListItemStatus status, string? measurementUnit, decimal? mesuredValue, bool isPass)
        {
            Status = status;
            MeasurementUnit = measurementUnit;
            MesuredValue = mesuredValue;
            IsPass = isPass;
            Evaluated = DateTime.UtcNow;
        }
    }
}
