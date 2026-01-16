using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.ExperimentLogEvent;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class ExperimentLogs : AuditableEntity
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
        public required string AssignedTo { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public ExperimentLogStatus Status { get; set; }
        //0 - Created: đã tạo, đã assign technician, CHƯA bắt đầu thực nghiệm
        //1 - InProgressed: đang thực hiện các stage
        //2 - Completed: hoàn thành
        //3 - Destroyed: hủy do toàn bộ sample nhiễm bệnh
        public virtual List<Samples> Samples { get; set; } = [];

        public void Start()
        {
            if (Status != ExperimentLogStatus.Created)
                throw new InvalidOperationException("Experiment log đã bắt đầu hoặc hoàn thành rồi.");

            Status = ExperimentLogStatus.InProgressed;
            CurrentStageOrder = 1;
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            AddDomainEvent(new ExperimentLogStarted(this.ID, MethodId, AssignedTo));
        }

        public void MoveToNextStage(MethodStages nextStage, int maxStageOrder)
        {
            EnsureInProgressed();

            if (CurrentStageOrder >= maxStageOrder)
                throw new DomainException("Đã ở giai đoạn cuối.");

            CurrentStageOrder++;

            AddDomainEvent(new ExperimentLogStageChanged(
                ID,
                CurrentStageOrder,
                AssignedTo
            ));

            if (nextStage.IsSampleGenerated)
            {
                AddDomainEvent(new ExperimentLogSampleGenerationRequired(
                    ID,
                    nextStage.ID,
                    CurrentStageOrder,
                    ExpectedSampleCount,
                    AssignedTo
                ));
            }
        }


        public void DestroyBecauseAllSamplesInfected(string? reason)
        {
            EnsureInProgressed();

            Status = ExperimentLogStatus.Destroyed;
            Reason = reason;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);

            AddDomainEvent(new ExperimentLogDestroyed(ID, reason));
        }

        public void UpdateInformation(string? name, string? notes)
        {
            Name = name ?? Name;
            Notes = notes;
        }

        public void Complete()
        {
            EnsureNotDestroyed();
            EnsureInProgressed();

            Status = ExperimentLogStatus.Completed;

            EndDate = DateOnly.FromDateTime(DateTime.Now);
            AddDomainEvent(new ExperimentLogCompleted(this.ID));
        }

        private void EnsureNotDestroyed()
        {
            if (Status == ExperimentLogStatus.Destroyed)
                throw new DomainException("Experiment log đã bị hủy.");

        }

        private void EnsureInProgressed()
        {
            if (Status != ExperimentLogStatus.InProgressed)
                throw new InvalidOperationException("Experiment log không trong quá trình thực hiện.");
        }
    }
}