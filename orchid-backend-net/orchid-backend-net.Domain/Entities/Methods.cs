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

        public MethodStages AddMethodStage(
            int stageDefinitionId,
            int order,
            int durationDays)
        {
            if (MethodStages.Any(ms => ms.MethodStageDefinitionId == stageDefinitionId))
                throw new DuplicateException("Đã tồn tại stage này trong method này rồi.");
            var stage = new MethodStages
            {
                MethodId = this.ID,
                MethodStageDefinitionId = stageDefinitionId,
                DurationsDays = durationDays,
                Order = order
            };

            MethodStages.Add(stage);
            return stage;
        }


        public void AddMethodStageWithResource(
            int stageDefinitionId,
            int order,
            int durationDays,
            IEnumerable<int>? materials,
            IEnumerable<int>? chemicals)
        {
            var stage = AddMethodStage(stageDefinitionId, order, durationDays);

            materials?.ToList().ForEach(stage.AddMaterial);
            chemicals?.ToList().ForEach(stage.AddChemical);
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

 
        private MethodStages GetStageOrThrow(int methodStageId)
        {
            return MethodStages.SingleOrDefault(s => s.ID == methodStageId)
                ?? throw new DomainException("Không tìm thấy stage.");
        }

    }
}