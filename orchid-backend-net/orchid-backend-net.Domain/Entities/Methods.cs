using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Methods : BaseIntEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<ExperimentLogs> ExperimentLogs { get; set; } = [];
        public virtual List<MethodStages> MethodStages { get; set; } = [];

        public void AddMethodStageToMethod(int stageDefinitionId, int order, int durationDays)
        {
            if (MethodStages.Any(ms => ms.StageDefinitionId == stageDefinitionId))
                throw new DuplicateException("Đã tồn tại stage này trong method này rồi.");
            MethodStages.Add(new MethodStages
            {
                MethodId = this.ID,
                StageDefinitionId = stageDefinitionId,
                DurationsDays = durationDays,
                Order = order
            });
        }

        public void AddMaterialToStage(int methodStageId, int materialId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.AddMaterial(materialId);
        }

        public void AddChemicalToStage(int methodStageId, int chemicalId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.AddChemical(chemicalId);
        }

        public void RemoveMaterialFromStage(int methodStageId, int materialId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.RemoveMaterial(materialId);
        }

        public void RemoveChemicalFromStage(int methodStageId, int chemicalId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.RemoveChemical(chemicalId);
        }

        public void UpdateMaterialInStage(int methodStageId, string stageMaterialId, int? materialId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.UpdateMaterial(stageMaterialId, materialId);
        }

        public void UpdateChemicalInStage(int methodStageId, string stageChemicalId, int? chemicalId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.UpdateChemical(stageChemicalId, chemicalId);
        }

        public void AddSampleRequirementToStage(int methodStageId, CreateSampleRequirementSpec spec)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.AddSampleRequirement(spec);
        }

        public void RemoveSampleRequirementFromStage(int methodStageId, string sampleRequirementId)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.RemoveSampleRequirement(sampleRequirementId);
        }

        public void UpdateSampleRequirementInStage(int methodStageId, string sampleRequirementId, UpdateSampleRequirementSpec spec)
        {
            var stage = GetStageOrThrow(methodStageId);
            stage.UpdateSampleRequirement(sampleRequirementId, spec);
        }

        private MethodStages GetStageOrThrow(int methodStageId)
        {
            return MethodStages.SingleOrDefault(s => s.ID == methodStageId)
                ?? throw new DomainException("Không tìm thấy stage.");
        }

    }
}