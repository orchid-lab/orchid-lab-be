using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.ExperimentLogEvent;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class ExperimentLogs : BaseGuidEntity
    {
        public required string HybridzationId { get; set; }
        public virtual Hybridzations Hybridzations { get; set; }
        public required int MethodId { get; set; }
        [ForeignKey(nameof(MethodId))]
        public virtual Methods Method { get; set; }
        public required int BatchId { get; set; }
        [ForeignKey(nameof(BatchId))]
        public virtual Batches Batch { get; set; }
        public int ExpectedSampleCount { get; set; }
        public int CurrentStageOrder { get; set; }
        public required string Name { get; set; }
        public required string CreatedBy { get; set; }
        public required string AssignedTo { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public ExperimentLogStatus Status { get; set; }
        //0 - Mới tạo - chưa nhận
        //1 - đang tiến hành - diễn ra khi technician nhận experiment log
        //2 - Hoàn thành
        //3 - Bị hủy => hủy toàn bộ samples thuộc experiment log này
        public virtual List<Samples> Samples { get; set; } = [];

        public void Start()
        {
            if (Status != ExperimentLogStatus.Created)
                throw new InvalidOperationException("Experiment log đã bắt đầu hoặc hoàn thành rồi.");

            Status = ExperimentLogStatus.InProgressed;
            CurrentStageOrder = 1;
            AddDomainEvent(new ExperimentLogStarted(this.ID, MethodId, AssignedTo));
        }

        public void MoveToNextStage(List<MethodStages> methodStages)
        {
            if (Status != ExperimentLogStatus.InProgressed)
                throw new InvalidOperationException("Experiment log không trong quá trình thực hiện.");

            var maxStageOrder = methodStages.Max(s => s.Order);
            if (CurrentStageOrder >= maxStageOrder)
                throw new InvalidOperationException("Đã ở giai đoạn cuối.");

            CurrentStageOrder++;

            var nextStage = methodStages.FirstOrDefault(s => s.Order == CurrentStageOrder);
            if (nextStage != null)
            {
                // Trigger domain event cho stage này (tạo Task, hoặc notify technician)
                AddDomainEvent(new ExperimentLogStageChanged(this.ID, CurrentStageOrder, AssignedTo));
            }
        }

        public void Complete()
        {
            Status = ExperimentLogStatus.Completed;
            EndDate = DateOnly.FromDateTime(DateTime.Now);
            AddDomainEvent(new ExperimentLogCompleted(this.ID));
        }

        public void UpdateInformation(string? name, string? notes)
        {
            Name = name ?? Name;
            Notes = notes;
        }

        public void MarkCompleted()
        {
            EnsureNotDestroyed();

            if (Status == ExperimentLogStatus.Completed)
                return;
            Status = ExperimentLogStatus.Completed;
        }

        public void DestroyExperimentLogBecauseOfAllSampleInfected(string? reason)
        {
            EnsureNotDestroyed();
            Reason = reason;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = ExperimentLogStatus.Destroyed;
        }

        private void EnsureNotDestroyed()
        {
            if (Status == ExperimentLogStatus.Destroyed)
                throw new DomainException("Experiment log đã bị hủy.");

        }
    }
}