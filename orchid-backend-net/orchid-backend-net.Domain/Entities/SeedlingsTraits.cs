using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SeedlingsTraits : BaseGuidEntity
    {
        public string SeedlingId { get; set; }
        [ForeignKey(nameof(SeedlingId))]
        public virtual Seedlings Seedling { get; set; }
        public required string CharacteristicId { get; set; }
        [ForeignKey(nameof(CharacteristicId))]
        public virtual Characteristic Characteristics { get; set; }
        public required decimal Value { get; set; }
    }
}