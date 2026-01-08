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
        public required int StageDefinitionId { get; set; }
        [ForeignKey(nameof(StageDefinitionId))]
        public virtual StageDefinition StageDefinition { get; set; }
        public int DurationsDays { get; set; }
        public int Order { get; set; }
        public virtual List<StageMaterials> StageMaterials { get; set; } = [];
        public virtual List<StageChemicals> StageChemicals { get; set; } = [];
        public virtual List<MethodStageSampleRequirement> SamplesRequirements { get; set; } = [];

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

        public void AddSampleRequirement(
            CreateSampleRequirementSpec spec)
        {
            ValidateRange(spec.MinValue, spec.MaxValue, spec.ExpectedValue);

            if (SamplesRequirements.Any(x => x.SampleRequirementId == spec.SampleRequirementId))
                throw new DuplicateException("Requirement cho characteristic này đã tồn tại.");

            SamplesRequirements.Add(new MethodStageSampleRequirement
            {
                SampleRequirementId = spec.SampleRequirementId,
                MinValue = spec.MinValue,
                MaxValue = spec.MaxValue,
                ExpectedValue = spec.ExpectedValue,
            });
        }

        public void RemoveSampleRequirement(string sampleRequirementId)
        {
            var sampleReq = GetSampleRequirementOrThrow(sampleRequirementId);
            SamplesRequirements.Remove(sampleReq);
        }

        public void UpdateSampleRequirement(
           string sampleRequirementId,
           UpdateSampleRequirementSpec spec)
        {
            var sampleReq = GetSampleRequirementOrThrow(sampleRequirementId);

            var min = spec.MinValue ?? sampleReq.MinValue;
            var max = spec.MaxValue ?? sampleReq.MaxValue;
            var expected = spec.ExpectedValue ?? sampleReq.ExpectedValue;

            ValidateRange(min, max, expected);

            sampleReq.MinValue = min;
            sampleReq.MaxValue = max;
            sampleReq.ExpectedValue = expected;
        }

        private MethodStageSampleRequirement GetSampleRequirementOrThrow(string id)
            => SamplesRequirements.SingleOrDefault(x => x.ID == id)
                ?? throw new DomainException("Sample Requirement không tồn tại");

        private static void ValidateRange(decimal min, decimal max, decimal expected)
        {
            if (min > max)
                throw new DomainException("Min value không thể lớn hơn Max value.");

            if (expected < min || expected > max)
                throw new DomainException("Expected value phải nằm trong khoảng Min–Max.");
        }
    }

    public sealed class UpdateSampleRequirementSpec
    {
        public decimal? MinValue { get; init; }
        public decimal? MaxValue { get; init; }
        public decimal? ExpectedValue { get; init; }
    }

    public sealed class CreateSampleRequirementSpec
    {
        public required string SampleRequirementId { get; init; }
        public required decimal MinValue { get; init; }
        public required decimal MaxValue { get; init; }
        public required decimal ExpectedValue { get; init; }
    }
}