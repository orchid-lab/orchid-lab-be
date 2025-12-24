using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Seedlings : BaseGuidEntity
    {
        public required string LocalName { get; set; }
        public required string ScientificName { get; set; }
        public string? Description { get; set; }
        public string? ParentAId { get; set; }
        public string? ParentBId { get; set; }
        [ForeignKey(nameof(ParentAId))]
        public virtual Seedlings? ParentA { get; set; }
        [ForeignKey(nameof(ParentBId))]
        public virtual Seedlings? ParentB { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public virtual IEnumerable<SeedlingsTraits> SeedlingsTraits { get; set; } = [];
    }
}