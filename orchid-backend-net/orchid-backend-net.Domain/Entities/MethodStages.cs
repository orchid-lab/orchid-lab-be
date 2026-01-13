using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class MethodStages : BaseIntEntity
    {
        public required int MethodId { get; set; }
        [ForeignKey(nameof(MethodId))]
        public virtual Methods Method { get; set; }
        public required int MethodStageDefinitionId { get; set; }
        [ForeignKey(nameof(MethodStageDefinitionId))]
        public virtual MethodStageDefinition MethodStageDefinition { get; set; }
        public int DurationsDays { get; set; }
        public int Order { get; set; }
        public virtual List<StageMaterials> StageMaterials { get; set; } = new();
        public virtual List<StageChemicals> StageChemicals { get; set; } = new();

        public void AddMaterial(int materialId)
        {
            if (StageMaterials.Any(m => m.MaterialId == materialId))
                throw new DomainException("Material này vốn dĩ đã tồn tại trong stage rồi");

            StageMaterials.Add(new StageMaterials()
            {
                StageId = this.ID,
                MaterialId = materialId
            });
        }

        public void RemoveMaterial(int materialId)
        {
            var stageMaterial = StageMaterials.SingleOrDefault(m => m.MaterialId == materialId)
                ?? throw new DomainException("Material này không tồn tại");
            StageMaterials.Remove(stageMaterial);
        }

        public void UpdateMaterial(string stageMaterialId, int? materialId)
        {

            var stageMaterial = StageMaterials.SingleOrDefault(m => m.ID == stageMaterialId)
                ?? throw new DomainException("Material này không tồn tại");
            stageMaterial.MaterialId = materialId ?? stageMaterial.MaterialId;
        }

        public void AddChemical(int chemicalId)
        {
            if (StageChemicals.Any(m => m.ChemicalId == chemicalId))
                throw new DomainException("Chemical này vốn dĩ đã tồn tại trong stage rồi");

            StageChemicals.Add(new StageChemicals()
            {
                StageId = this.ID,
                ChemicalId = chemicalId
            });
        }

        public void RemoveChemical(int chemicalId)
        {
            var stageChemical = StageChemicals.SingleOrDefault(m => m.ChemicalId == chemicalId)
                ?? throw new DomainException("Chemical này không tồn tại");
            StageChemicals.Remove(stageChemical);
        }

        public void UpdateChemical(string stageChemicalId, int? chemicalId)
        {

            var stageChemical = StageChemicals.SingleOrDefault(m => m.ID == stageChemicalId)
                ?? throw new DomainException("Chemical này không tồn tại");
            stageChemical.ChemicalId = chemicalId ?? stageChemical.ChemicalId;
        }
    }

   
}