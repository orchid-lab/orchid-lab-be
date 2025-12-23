using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Materials : BaseIntEntity
    {
        public required string Name { get; set; }
        public required string Catetgory { get; set; }
        public string? Description { get; set; }
        public required string Unit { get; set; }
        public virtual IEnumerable<StageMaterials> StageMaterials { get; set; } = [];
    }
}