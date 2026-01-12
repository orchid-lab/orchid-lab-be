using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Batches : BaseIntEntity
    {
        public required int LabRoomId { get; set; }
        [ForeignKey(nameof(LabRoomId))]
        public required virtual LabRooms LabRoom { get; set; }
        public string BatchName { get; set; } = default!;
        public int BatchSize { get; set; } = default!;
        public bool IsBatching { get; set; } = false;   
    }
}