using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SamplesRequirements : BaseGuidEntity
    {
        public int StageId { get; set; }
        [ForeignKey(nameof(StageId))]
        public virtual Stages Stage { get; set; }
        //only using for reference, not foreign key
        //normally would link to Characteristic entity, but to reduce complexity and dependencies, just store code here
        //not showing to user, just for internal linking
        public string? CharacteristicCode { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal MinValue { get; set; }
        public required decimal MaxValue { get; set; }
        public required decimal ExpectedValue { get; set; } 
        public required string Unit { get; set; }
    }
}