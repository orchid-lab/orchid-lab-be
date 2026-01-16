using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Seedlings : AuditableEntity
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
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual List<SeedlingsTraits> SeedlingsTraits { get; set; } = [];

        public void UpdateTrait(string traitId, decimal value)
        {
            var trait = SeedlingsTraits.FirstOrDefault(t => t.ID == traitId)
                ?? throw new NotFoundException("Trait không tồn tại.");

            trait.Value = value;
        }

        public void AddTrait(string characteristicId, decimal value)
        {
            if (SeedlingsTraits.Any(t => t.CharacteristicId == characteristicId))
                throw new DuplicateException("Trait đã tồn tại.");

            SeedlingsTraits.Add(new SeedlingsTraits
            {
                CharacteristicId = characteristicId,
                Value = value
            });
        }
    }
}