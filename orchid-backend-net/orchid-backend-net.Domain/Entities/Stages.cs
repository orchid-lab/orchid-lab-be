using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Stages : BaseIntEntity
    {
        public required int MethodId { get; set; }
        [ForeignKey(nameof(MethodId))]
        public virtual Methods Method { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public virtual IEnumerable<StageMaterials> StageMaterials { get; set; } = [];
        public virtual IEnumerable<StageChemicals> StageChemicals { get; set; } = [];
        public virtual IEnumerable<SamplesRequirements> SamplesRequirements { get; set; } = [];
    }
}