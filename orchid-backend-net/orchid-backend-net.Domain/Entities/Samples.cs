using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Samples : AuditableEntity
    {
        public string Name { get; set; } = null!;
        public required string ExperimentLogId { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly? ExecutionDate { get; set; }
        public string? InitialCondition { get; set; }   // Trạng thái ban đầu, vd: "mẫu xanh tươi, dài 2cm"

        [ForeignKey(nameof(ExperimentLogId))]
        public virtual ExperimentLogs ExperimentLog { get; set; } = null!;
        public virtual List<SampleStage> SampleStages { get; set; } = new();

        /// <summary>
        /// create first stage and start it
        /// </summary>
        /// <param name="firstStageDefinitionId">id of sample stage definition</param>
        public void StartOnCreation(int firstStageDefinitionId)
        {
            EnsureSampleIsActive();

            var alreadyInProgress = SampleStages
                .Any(s => s.Status == SampleStatus.InProgressed);
            if (alreadyInProgress)
                return;

            var firstStage = new SampleStage
            {
                SampleStageDefinitionId = firstStageDefinitionId,
                SampleId = ID,
                Status = SampleStatus.Created,
            };
            firstStage.Start();
            SampleStages.Add(firstStage);
        }

        /// <summary>
        /// only using for update information of sample when it is in progress
        /// like name, notes, reason
        /// </summary>
        /// <param name="name"></param>
        /// <param name="notes"></param>
        /// <param name="reason"></param>
        public void UpdateSampleInformation(string? name, string? notes, string? reason)
        {
            var currentStage = GetCurrentSampleStage();

            currentStage.EnsureNotTerminated();

            Name = name ?? Name;
            Notes = notes ?? Notes;
            Reason = reason ?? Reason;
        }

        /// <summary>
        /// if sample is in disease, cancel it
        /// </summary>
        /// <param name="reason"></param>
        public void CancelBecauseOfDisease(string? reason)
        {
            EnsureSampleIsActive();

            var currentStage = GetCurrentSampleStage();

            ExecutionDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Reason = reason;
            currentStage.MarkAsExecuted();

        }

        /// <summary>
        /// finish the sample current stage and move onto the next stage 
        /// </summary>
        /// <param name="definitionOrderMap"></param>
        /// <param name="orderDefinitionIds"></param>
        public void CompleteCurrentStage(IReadOnlyList<int> orderDefinitionIds)
        {
            if (orderDefinitionIds is null || orderDefinitionIds.Count == 0)
                throw new DomainException("Danh sách định nghĩa giai đoạn không được rỗng.");

            EnsureSampleIsActive();

            var currentStage = GetCurrentSampleStage();
            var currentIndex = IndexOfDefinition(orderDefinitionIds, currentStage.SampleStageDefinitionId);

            currentStage.MarkAsCompleted();

            var isLastStage = currentIndex == orderDefinitionIds.Count - 1;
            if (!isLastStage)
            {
                MoveToNextStage(orderDefinitionIds, currentIndex);
            }
        }

        private void MoveToNextStage(
            IReadOnlyList<int> orderedDefinitionIds,
            int currentIndex
            )
        {
            var nextDefinitionId = orderedDefinitionIds[currentIndex + 1];

            var nextStage = new SampleStage
            {
                SampleStageDefinitionId = nextDefinitionId,
                SampleId = ID,
                Status = SampleStatus.Created
            };

            nextStage.Start();
            SampleStages.Add(nextStage);
        }


        private static int IndexOfDefinition(
            IReadOnlyList<int> orderDefinitionIds,
            int definitionId)
        {
            var index = -1;
            for(int i = 0; i < orderDefinitionIds.Count; i++)
            {
                if(orderDefinitionIds[i] == definitionId)
                {
                    index = i;
                    break;
                }
            }
            if(index == -1) 
                throw new DomainException("Định nghĩa giai đoạn không tồn tại trong danh sách định nghĩa giai đoạn.");
            return index;
        }

        public void ConvertToSeedling()
        {
            EnsureSampleIsActive();

            var hasCompletedStage = SampleStages
                .Any(s => s.Status == SampleStatus.Completed);

            if (!hasCompletedStage)
                throw new DomainException("Chỉ có thể chuyển mẫu đã hoàn thành ít nhất một giai đoạn thành cây giống.");

            var stillInProgress = SampleStages
                .Any(s => s.Status == SampleStatus.InProgressed);

            if (stillInProgress)
                throw new DomainException("Mẫu vẫn còn đang ở giai đoạn chưa hoàn thành, không thể chuyển thành cây giống.");

            var lastStage = SampleStages
                .OrderByDescending(s => s.StartedAt)
                .First();

            lastStage.MarkAsConvertedToSeedling();
        }

        private SampleStage GetCurrentSampleStage()
        {
            var stage = SampleStages.SingleOrDefault(
                s => s.Status == Common.Enum.SampleStatus.InProgressed);
            return stage is null ? throw new DomainException("Sample không có stage đang tiến hành.") : stage;
        }

        private void EnsureSampleIsActive()
        {
            if (ExecutionDate.HasValue)
                throw new DomainException("Sample đã bị hủy, không thể thao tác tiếp.");
        }
    }
}