using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Batches : BaseIntEntity
    {
        public required int LabRoomId { get; set; }
        [ForeignKey(nameof(LabRoomId))]
        public required virtual LabRooms LabRoom { get; set; }
    }
}