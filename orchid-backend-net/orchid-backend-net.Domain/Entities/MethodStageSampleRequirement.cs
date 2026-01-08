using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class MethodStageSampleRequirement : BaseGuidEntity
    {
        public int MethodStageId { get; set; }
        [ForeignKey(nameof(MethodStageId))]
        public virtual MethodStages MethodStages { get; set; }
        public string SampleRequirementId { get; set; }
        [ForeignKey(nameof(SampleRequirementId))]
        public virtual SamplesRequirementsDefinition SampleRequirementsDefinition { get; set; }
        public required decimal MinValue { get; set; }
        public required decimal MaxValue { get; set; }
        public required decimal ExpectedValue { get; set; }
    }
}
