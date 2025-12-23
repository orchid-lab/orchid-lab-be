using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SamplesRequirements : BaseGuidEntity
    {
        public int StageId { get; set; }
        [ForeignKey(nameof(StageId))]
        public virtual Stages Stage { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal MinValue { get; set; }
        public required decimal MaxValue { get; set; }
        public required decimal ExpectedValue { get; set; } 
        public required string Unit { get; set; }
    }
}