using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SampleStageRequirement : BaseGuidEntity
    {
        public required string SampleStageId { get; set; }
        [ForeignKey(nameof(SampleStageId))]
        public virtual SampleStage SampleStage { get; set; } = default!;
        public required string SampleRequirementDefinitionId { get; set; }
        [ForeignKey(nameof(SampleRequirementDefinitionId))]
        public virtual SamplesRequirementsDefinition SampleRequirementsDefinition { get; set; } = default!;
        public required decimal ExpectedValue { get; set; }
    }
}
