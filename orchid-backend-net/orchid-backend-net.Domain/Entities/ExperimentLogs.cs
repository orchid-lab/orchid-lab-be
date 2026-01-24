using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class ExperimentLogs : AuditableEntity
    {
        public string SeedlingParentId { get; set; }
        [ForeignKey(nameof(SeedlingParentId))]
        public virtual Seedlings SeedlingParent { get; set; }
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
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public ExperimentLogStatus Status { get; set; }
        //0 - Created: đã tạo, đã assign technician, CHƯA bắt đầu thực nghiệm
        //1 - InProgressed: đang thực hiện các stage
        //2 - Completed: hoàn thành
        //3 - Destroyed: hủy do toàn bộ sample nhiễm bệnh
        public virtual List<Samples> Samples { get; set; } = new();

        public void Start()
        {
            if (Status != ExperimentLogStatus.Created)
                throw new InvalidOperationException("Experiment log đã bắt đầu hoặc hoàn thành rồi.");

            if (MethodId <= 0)
                throw new DomainException("Method must be specified before starting experiment.");

            if (BatchId <= 0)
                throw new DomainException("Batch must be specified before starting experiment.");

            if (string.IsNullOrWhiteSpace(AssignedTo))
                throw new DomainException("AssignedTo (technician) must be specified before starting experiment.");

            if (ExpectedSampleCount < 0)
                throw new DomainException("ExpectedSampleCount cannot be negative.");

            Status = ExperimentLogStatus.InProgress;
            CurrentStageOrder = 1;
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow);

            Batch.StartBatching();

            AddDomainEvent(new ExperimentLogStarted(
                this.ID, 
                BatchId, 
                AssignedTo,
                CreatedBy));

            AddDomainEvent(new SeedTaskOnStartExperimentLogEvent(
                ID,
                Method.ID,
                AssignedTo,
                CreatedBy
            ));
        }

        public void PendingToChangeStage()
        {
            EnsureInProgressed();
            Status = ExperimentLogStatus.WaitingForChangeStage;
            AddDomainEvent(new ExperimentLogPendingToChangeStage(
                ID,
                CurrentStageOrder,
                AssignedTo
            ));
        }

        public void MoveToNextStage(MethodStages nextStage, int maxStageOrder)
        {
            EnsureInWaitingForChangeStage();

            if (CurrentStageOrder >= maxStageOrder)
                throw new DomainException("Đã ở giai đoạn cuối.");

            CurrentStageOrder++;
            Status = ExperimentLogStatus.InProgress;
            AddDomainEvent(new ExperimentLogStageChanged(
                ID,
                CurrentStageOrder,
                AssignedTo
            ));

            AddDomainEvent(new SeedTaskOnExperimentLogStageChanged(
                ID,
                MethodId,
                AssignedTo,
                CreatedBy));

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
            EnsureInProgressOrWaiting();

            Status = ExperimentLogStatus.Destroyed;
            Reason = reason;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            AddDomainEvent(new ExperimentLogDestroyed(ID, reason));
            Batch.FinishBatching(CreatedBy);
        }

        public void UpdateInformation(string? name, string? notes, int? expectedSampleCount)
        {
            EnsureNotFinished();
            Name = name ?? Name;
            Notes = notes;
            var currentStage = Method.MethodStages
                .FirstOrDefault(ms => ms.Order == CurrentStageOrder)
                ?? throw new DomainException("Không tìm thấy giai đoạn này");
            if (expectedSampleCount is not null
                &&
                currentStage.IsSampleGenerated)
            {
                throw new DomainException("Không thể cập nhật số lượng sample mong muốn sau khi mẫu đã được tạo.");
            }
            ExpectedSampleCount = expectedSampleCount.Value;
        }

        public void Complete()
        {
            EnsureNotDestroyed();
            EnsureInProgressOrWaiting();

            Status = ExperimentLogStatus.Completed;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            AddDomainEvent(new ExperimentLogCompleted(this.ID));
            Batch.FinishBatching(CreatedBy);
        }

        private void EnsureNotDestroyed()
        {
            if (Status == ExperimentLogStatus.Destroyed)
                throw new DomainException("Experiment log đã bị hủy.");

        }

        private void EnsureInProgressed()
        {
            if (Status != ExperimentLogStatus.InProgress)
                throw new InvalidOperationException("Experiment log không trong quá trình thực hiện.");
        }

        private void EnsureInProgressOrWaiting()
        {
            if (Status != ExperimentLogStatus.InProgress
                && Status != ExperimentLogStatus.WaitingForChangeStage)
            {
                throw new InvalidOperationException("Experiment log không trong trạng thái hợp lệ để thực hiện thao tác này.");
            }
        }

        private void EnsureInWaitingForChangeStage()
        {
            if (Status != ExperimentLogStatus.WaitingForChangeStage)
                throw new InvalidOperationException("Experiment log không ở trạng thái chờ chuyển giai đoạn.");
        }

        private void EnsureNotFinished()
        {
            if (Status == ExperimentLogStatus.Completed || Status == ExperimentLogStatus.Destroyed)
                throw new DomainException("Không thể cập nhật experiment log đã hoàn thành hoặc bị hủy.");
        }
    }
}