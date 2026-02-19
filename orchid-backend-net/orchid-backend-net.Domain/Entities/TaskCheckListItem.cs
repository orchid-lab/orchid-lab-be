using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskCheckListItem : BaseGuidEntity
    {
        public string TaskCheckListId { get; set; }
        public virtual TaskCheckList TaskCheckList { get; set; } = null!;
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }

        //Optional standard metadata
        public string? ExpectedUnit { get; set; }
        public decimal? ExpectedMinValue { get; set; }
        public decimal? ExpectedMaxValue { get; set; }

        //Technician fills in measurment result 
        public TaskCheckListItemStatus Status { get; set; } = TaskCheckListItemStatus.Pending;
        public string? MeasurementUnit { get; set; }
        public decimal? MesuredValue { get; set; }

        //Evaluator fills in evaluation result
        public bool? IsPass { get; set; }
        public DateTime? Evaluated { get; set; }


        internal void Start()
        {
            Status = TaskCheckListItemStatus.InProgress;
        }


        /// <summary>
        /// technician submit measurment result, this action will set status to complete, and update mesured value and unit
        /// </summary>
        /// <param name="measuredValue"></param>
        /// <param name="measurementUnit"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal void SubmitByTechnician(decimal? measuredValue, string? measurementUnit)
        {
            if (Status != TaskCheckListItemStatus.Pending)
                throw new InvalidOperationException("Checklist item đã được submit hoặc đánh giá.");
            if(ExpectedUnit != null && measurementUnit != ExpectedUnit)
                throw new InvalidOperationException($"Đơn vị đo lường không hợp lệ. Yêu cầu: {ExpectedUnit}.");
            MesuredValue = measuredValue;
            MeasurementUnit = measurementUnit;
            Status = TaskCheckListItemStatus.Complete;
        }

        /// <summary>
        /// Researcher evaluate the checklist item after technician submit, this action will update IsPass and Evaluated date
        /// </summary>
        /// <param name="isPass"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal void EvaluateByResearcher(bool isPass)
        {
            if (Status != TaskCheckListItemStatus.Complete)
                throw new InvalidOperationException("Checklist item chưa được submit bởi technician.");
            IsPass = isPass;
            if(!isPass)
                Status = TaskCheckListItemStatus.Failed;
            Evaluated = DateTime.UtcNow;
        }

        internal void ResetForRework()
        {
            Status = TaskCheckListItemStatus.Pending;
            MeasurementUnit = null;
            MesuredValue = null;
            IsPass = null;
            Evaluated = null;
        }
    }
}
