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
        public int CurrentStageOrder { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly? ExecutionDate { get; set; }
        public SampleStatus Status { get; set; }
        //0 - Mới tạo - technician chưa nhận experiment log để tiến hành lai tạo
        //1 - Đang tiến hành - diễn ra khi technician nhận experiment log
        //2 - Hoàn thành
        //3 - Bị hủy 
        [ForeignKey(nameof(ExperimentLogId))]
        public virtual ExperimentLogs ExperimentLog { get; set; } = null!;

        public void MoveToNextStage(IReadOnlyCollection<MethodStages> methodStages)
        {
            EnsureNotTerminated();

            if (methodStages == null || methodStages.Count == 0)
                throw new DomainException("Method không có stage nào.");

            var maxOrder = methodStages.Max(s => s.Order);

            if (CurrentStageOrder >= maxOrder)
                throw new DomainException("Sample đã ở stage cuối.");

            CurrentStageOrder++;

            if (Status == SampleStatus.Created)
                Status = SampleStatus.InProgressed;
        }


        public void UpdateInformation(string? notes, string? reason)
        {
            EnsureNotTerminated();
            Notes = notes ?? Notes;
            Reason = reason ?? Reason;
        }

        public void MarkCompleted()
        {
            EnsureNotTerminated();

            if (Status == SampleStatus.Completed)
                return;

            Status = SampleStatus.Completed;
        }

        public void CancelBecauseOfDisease(string? reason)
        {
            ExecutionDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = SampleStatus.ExecutedBecauseOfDisease;
            Reason = reason;
        }

        public SeedlingConversionResult ConvertToSeedling(
            IReadOnlyCollection<MethodStages> methodStages,
            IReadOnlyCollection<MonitoringLogs> approvedLogs,
            Hybridzations hybridzation)
        {
            EnsureConvertible(methodStages);

            var traits = BuildTraitDrafts(approvedLogs);

            if (traits.Count == 0)
                throw new DomainException("Không có trait hợp lệ để convert.");

            Status = SampleStatus.ConvertedToSeedling;

            return new SeedlingConversionResult(
                SampleId: ID,
                ExperimentLogId: ExperimentLogId,
                ParentAId: hybridzation.ParentAId,
                ParentBId: hybridzation.ParentBId,
                Traits: traits
            );
        }

        private void EnsureNotTerminated()
        {
            if(Status == SampleStatus.ExecutedBecauseOfDisease)
                throw new DomainException("Sample đã bị hủy vì bệnh.");
        }

        private void EnsureConvertible(
            IReadOnlyCollection<MethodStages> methodStages)
        {
            if (methodStages == null || methodStages.Count == 0)
                throw new DomainException("Method không có stage nào.");

            var finalStageOrder = methodStages.Max(s => s.Order);

            if (CurrentStageOrder != finalStageOrder)
                throw new DomainException("Sample chưa ở stage cuối của method.");

            if (Status == SampleStatus.ConvertedToSeedling)
                throw new DomainException("Sample đã được convert sang Seedling.");

            if (Status != SampleStatus.Completed)
                throw new DomainException("Sample chưa ở trạng thái Completed.");
        }

        private static IReadOnlyCollection<SeedlingTraitDraft> BuildTraitDrafts(
            IReadOnlyCollection<MonitoringLogs> approvedLogs)
        {
            if (approvedLogs == null || approvedLogs.Count == 0)
                return [];
            return [..approvedLogs
                .SelectMany(log => log.LogDetails)
                .Where(d => d.IsMatch)
                .Select(d =>
                {
                    var code = d.Requirement
                        .SampleRequirementsDefinition
                        .CharacteristicCode;

                    if (string.IsNullOrWhiteSpace(code))
                        return null;

                    return new SeedlingTraitDraft(
                        characteristicCode: code,
                        value: d.MeasuredValue
                    );
                })
                .Where(d => d is not null)
                .DistinctBy(d => d!.CharacteristicCode)!];
        }
    }
    //value object 
    public sealed record SeedlingConversionResult(
    string SampleId,
    string ExperimentLogId,
    string? ParentAId,
    string? ParentBId,
    IReadOnlyCollection<SeedlingTraitDraft> Traits)
    {
        public static SeedlingConversionResult Create(
            Samples sample,
            Hybridzations hybridzation,
            IReadOnlyCollection<SeedlingTraitDraft> traits)
            => new(
                SampleId: sample.ID,
                ExperimentLogId: sample.ExperimentLogId,
                ParentAId: hybridzation.ParentAId,
                ParentBId: hybridzation.ParentBId,
                Traits: traits
            );
    }


    public sealed class SeedlingTraitDraft
    {
        public string CharacteristicCode { get; }
        public decimal Value { get; }

        public SeedlingTraitDraft(string characteristicCode, decimal value)
        {
            CharacteristicCode = characteristicCode;
            Value = value;
        }
    }
}