using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class LabRooms : BaseIntEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public virtual IEnumerable<Batches> Batches { get; set; } = [];
    }
}