using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Hybridzations : BaseGuidEntity
    {
        public required string ParentAId { get; set; } 
        public required string ParentBId { get; set; }
        [ForeignKey(nameof(ParentAId))]
        public virtual Seedlings ParentA { get; set; }
        [ForeignKey(nameof(ParentBId))]
        public virtual Seedlings ParentB { get; set; }
    }
}