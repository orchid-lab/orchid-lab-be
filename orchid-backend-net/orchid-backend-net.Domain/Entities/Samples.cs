using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Samples : BaseGuidEntity
    {
        public string Name { get; set; } = null!;
        public required string ExperimentLogId { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly? ExecutionDate { get; set; }

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
        public void CompleteCurrentStage( IReadOnlyList<int> orderDefinitionIds)
        {
            if(orderDefinitionIds.Count == 0 || orderDefinitionIds is null)
                throw new DomainException("Danh sách định nghĩa giai đoạn không được rỗng.");
            
            EnsureSampleIsActive();
            
            var currentStage = GetCurrentSampleStage();

            currentStage.MarkAsCompleted();
            MoveToNextStage(currentStage, orderDefinitionIds);
        }

        private void MoveToNextStage(
           SampleStage completedStage,
           IReadOnlyList<int> orderedDefinitionIds)
        {
            var currentIndex = IndexOfDefinition(orderedDefinitionIds, completedStage.SampleStageDefinitionId);

            // Không có stage tiếp theo
            if (currentIndex == orderedDefinitionIds.Count - 1)
                throw new DomainException("Sample này đã ở giai đoạn cuối cùng.");

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