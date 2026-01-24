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
        public void UpdateSampleInformation(string? name, string? notes, string? reason)
        {
            var currentStage = GetCurrentSampleStage();

            currentStage.EnsureNotTerminated();

            Name = name ?? Name;
            Notes = notes ?? Notes;
            Reason = reason ?? Reason;
        }

        public void CancelBecauseOfDisease(string? reason)
        {
            EnsureSampleIsActive();

            var currentStage = GetCurrentSampleStage();

            ExecutionDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Reason = reason;
            currentStage.MarkAsExecuted();

        }

        public void CompleteCurrentStage(
            IReadOnlyDictionary<int, int> definitionOrderMap,
            IReadOnlyList<int> orderDefinitionIds)
        {
            EnsureSampleIsActive();
            var currentStage = GetCurrentSampleStage();

            currentStage.MarkAsCompleted();
            MoveToNextStage(currentStage, definitionOrderMap, orderDefinitionIds);
        }

        public bool IsInFinalStage(
            IReadOnlyDictionary<int, int> definitionOrderMap,
            int maxStageOrder)
        {
            var currentStage = GetCurrentSampleStage();

            var currentOrder =
                definitionOrderMap[currentStage.SampleStageDefinitionId];

            return currentOrder == maxStageOrder;
        }

        private void MoveToNextStage(
            SampleStage completedStage,
            IReadOnlyDictionary<int, int> definitionOrderMap,
            IReadOnlyList<int> orderDefinitionIds)
        {
            var currentOrder = definitionOrderMap[completedStage.SampleStageDefinitionId];

            var nextDefinitionId = orderDefinitionIds
                .FirstOrDefault(id => definitionOrderMap[id] > currentOrder);

            if (nextDefinitionId == 0)
                return;

            if (IsInFinalStage(definitionOrderMap, currentOrder))
                throw new DomainException("Sample này đã phát triển tối đa rồi");

            var nextStage = new SampleStage
            {
                SampleStageDefinitionId = nextDefinitionId,
                SampleId = ID,
                Status = Common.Enum.SampleStatus.Created,
            };
            nextStage.Start();
            SampleStages.Add(nextStage);

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