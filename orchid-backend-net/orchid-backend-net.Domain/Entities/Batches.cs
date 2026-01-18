using orchid_backend_net.Domain.Common.Exceptions;
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

        public void StartBatching()
        {
            if (IsBatching)
                throw new DomainException("Batch đang được sử dụng rồi");
            IsBatching = true;
        }

        public void FihishBatching()
        {
            if (!IsBatching)
                throw new DomainException("Batch chưa được sử dụng");
            IsBatching = false;
        }
    }
}