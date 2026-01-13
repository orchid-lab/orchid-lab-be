using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SampleStageRequirement : BaseGuidEntity
    {
        public required string SampleStageId { get; set; }
        [ForeignKey(nameof(SampleStageId))]
        public virtual SampleStage SampleStage { get; set; } = default!;
        public required string StageRequirementDefinitionId { get; set; }
        [ForeignKey(nameof(StageRequirementDefinitionId))]
        public virtual StageRequirementDefinition StageRequirementDefinition { get; set; } = default!;
    }
}
