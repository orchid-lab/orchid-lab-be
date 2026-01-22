using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class LabRooms : BaseIntEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public virtual List<Batches> Batches { get; set; } = new();
    }
}