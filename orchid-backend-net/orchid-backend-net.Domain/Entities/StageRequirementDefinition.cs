using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class StageRequirementDefinition : BaseGuidEntity
    {
        public required int SampleStageDefinitionId { get; set; }
        [ForeignKey(nameof(SampleStageDefinitionId))]
        public virtual SampleStageDefinition SampleStage { get; set; } = default!;
        public required string SampleRequirementDefinitionId { get; set; }
        [ForeignKey(nameof(SampleRequirementDefinitionId))]
        public virtual SamplesRequirementsDefinition SampleRequirementsDefinition { get; set; } = default!;
        public required decimal ExpectedValue { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }
}
