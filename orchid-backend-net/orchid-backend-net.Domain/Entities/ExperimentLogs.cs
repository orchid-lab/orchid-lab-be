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
        public string? Objective { get; set; }          // Mục tiêu thí nghiệm (Mục 1)
        public string? Conclusion { get; set; }         // Đánh giá chung (Mục 10)
        public string? Issues { get; set; }             // Vấn đề gặp phải (Mục 10)
        public string? Recommendations { get; set; }    // Đề xuất / Điều chỉnh (Mục 10)
        public ExperimentLogStatus Status { get; set; }
        //0 - Created: đã tạo, đã assign technician, CHƯA bắt đầu thực nghiệm
        //1 - InProgressed: đang thực hiện các stage
        //2 - Completed: hoàn thành
        //3 - Destroyed: hủy do toàn bộ sample nhiễm bệnh
        public virtual List<Samples> Samples { get; set; } = new();

        #region Aggregate Root Methods
        /// <summary>
        /// using this to start experiment log, which will set start date, status, and trigger domain events to create seed tasks for the first stage.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="DomainException"></exception>
        public void Start()
        {
            EnsureStatusIs(ExperimentLogStatus.Created);

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

        /// <summary>
        /// this method is used when technician want to move to next stage, 
        /// so they need to pending the experiment log to waiting for change stage, which will trigger domain event to notify researcher to confirm the change stage.
        /// </summary>
        public void PendingToChangeStage()
        {
            EnsureStatusIs(ExperimentLogStatus.InProgress);
            Status = ExperimentLogStatus.WaitingForChangeStage;
            AddDomainEvent(new ExperimentLogPendingToChangeStage(
                ID,
                CurrentStageOrder,
                AssignedTo,
                CreatedBy
            ));
        }

        /// <summary>
        /// researcher confirm changing stage
        /// </summary>
        /// <param name="nextStage"></param>
        /// <param name="maxStageOrder"></param>
        /// <exception cref="DomainException"></exception>
        public void MoveToNextStage(MethodStages nextStage, int maxStageOrder)
        {
            EnsureStatusIs(ExperimentLogStatus.WaitingForChangeStage);

            if (CurrentStageOrder >= maxStageOrder)
                throw new DomainException("Đã ở giai đoạn cuối.");

            CurrentStageOrder++;
            Status = ExperimentLogStatus.InProgress;
            AddDomainEvent(new ExperimentLogStageChanged(
                ID,
                CurrentStageOrder,
                AssignedTo,
                CreatedBy
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

        /// <summary>
        /// when experiment log is full of infected sameple, technician can destroy the experiment log to avoid wasting time and resources on the next stages,
        /// which will set status to destroyed, set end date, and trigger domain event to notify researcher and other related entities to stop the experiment log.
        /// </summary>
        /// <param name="reason"></param>
        public void DestroyBecauseAllSamplesInfected(string? reason, string conclusion, string issue, string recommendation)
        {
            EnsureStatusIsOneOf(ExperimentLogStatus.InProgress, ExperimentLogStatus.WaitingForChangeStage);

            Status = ExperimentLogStatus.Destroyed;
            Reason = reason;
            Conclusion = conclusion;
            Issues = issue;
            Recommendations = recommendation;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            AddDomainEvent(new ExperimentLogDestroyed(ID, reason));
            Batch.FinishBatching(CreatedBy);
        }

        /// <summary>
        /// only using for update information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="notes"></param>
        /// <param name="expectedSampleCount"></param>
        /// <param name="objective"></param>
        /// <exception cref="DomainException"></exception>
        public void UpdateInformation(string? name, string? notes, int? expectedSampleCount, string? objective)
        {
            EnsureStatusIsNotOneOf(ExperimentLogStatus.Completed, ExperimentLogStatus.Destroyed);
            Name = name ?? Name;
            Notes = notes;
            Objective = objective ?? Objective;
            var currentStage = Method.MethodStages
                .FirstOrDefault(ms => ms.Order == CurrentStageOrder)
                ?? throw new DomainException("Không tìm thấy giai đoạn này");
            if (expectedSampleCount is not null
                &&
                currentStage.IsSampleGenerated)
            {
                throw new DomainException("Không thể cập nhật số lượng sample mong muốn sau khi mẫu đã được tạo.");
            }
            ExpectedSampleCount = expectedSampleCount ?? ExpectedSampleCount;
        }

        /// <summary>
        /// researcher complete the experiment log when all stages are completed, 
        /// which will set status to completed, 
        /// set end date, and trigger domain event to notify related entities to finish the experiment log.
        /// </summary>
        public void Complete(string? conclusion, string? issues, string? recommendations)
        {
            EnsureStatusIsNot(ExperimentLogStatus.Destroyed);
            EnsureStatusIsOneOf(ExperimentLogStatus.InProgress, ExperimentLogStatus.WaitingForChangeStage);
            Conclusion = conclusion;
            Issues = issues;
            Recommendations = recommendations;
            Status = ExperimentLogStatus.Completed;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            AddDomainEvent(new ExperimentLogCompleted(this.ID, this.AssignedTo, this.BatchId));
            Batch.FinishBatching(CreatedBy);
        }

        /// <summary>
        /// cancel is use by technician when he/she want to stop the experiment log for some reason,
        /// but the experiment log is not full of infected sample, 
        /// so they don't want to destroy it, just want to cancel it and keep the record of the experiment log.
        /// Only using when the experiment log is in created status, 
        /// which means the experiment log is not started yet, 
        /// so it won't trigger any domain event to stop the experiment log, just set status to cancelled and set end date.
        /// </summary>
        /// <param name="reason"></param>
        public void Cancel(string? reason)
        {
            EnsureStatusIsNot(ExperimentLogStatus.Destroyed);
            EnsureStatusIs(ExperimentLogStatus.Created);
            Status = ExperimentLogStatus.Cancelled;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Reason = reason;
            AddDomainEvent(new ExperimentLogCancel(ID, reason));
            Batch.FinishBatching(AssignedTo);
        }
        #endregion Aggregate Root Methods

        #region Validation Helpers

        /// <summary>
        /// Ensures that the experiment log is in the specified status.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the status does not match.</exception>
        private void EnsureStatusIs(ExperimentLogStatus expectedStatus)
        {
            if (Status != expectedStatus)
                throw new InvalidOperationException($"Experiment log không ở trạng thái {expectedStatus}.");
        }

        /// <summary>
        /// Ensures that the experiment log is in one of the specified statuses.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the status does not match any of the allowed statuses.</exception>
        private void EnsureStatusIsOneOf(params ExperimentLogStatus[] allowedStatuses)
        {
            if (!allowedStatuses.Contains(Status))
                throw new InvalidOperationException("Experiment log không ở trạng thái hợp lệ để thực hiện thao tác này.");
        }

        /// <summary>
        /// Ensures that the experiment log is NOT in the specified status.
        /// </summary>
        /// <exception cref="DomainException">Thrown if the status matches the forbidden status.</exception>
        private void EnsureStatusIsNot(ExperimentLogStatus forbiddenStatus)
        {
            if (Status == forbiddenStatus)
                throw new DomainException($"Experiment log đã ở trạng thái {forbiddenStatus}.");
        }

        /// <summary>
        /// Ensures that the experiment log is NOT in any of the specified statuses.
        /// </summary>
        /// <exception cref="DomainException">Thrown if the status matches any of the forbidden statuses.</exception>
        private void EnsureStatusIsNotOneOf(params ExperimentLogStatus[] forbiddenStatuses)
        {
            if (forbiddenStatuses.Contains(Status))
                throw new DomainException("Không thể cập nhật experiment log đã hoàn thành hoặc bị hủy.");
        }

        #endregion
    }
}