using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Chemicals : BaseIntEntity
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
        public required string ConcentrationUnit { get; set; } 
        public virtual IEnumerable<StageChemicals> StageChemicals { get; set; } = [];
    }
}