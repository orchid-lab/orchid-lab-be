using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.BatchEvent;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Batches : BaseIntEntity
    {
        public required int LabRoomId { get; set; }
        [ForeignKey(nameof(LabRoomId))]
        public virtual LabRooms LabRoom { get; set; }
        public string BatchName { get; set; } = default!;
        public decimal BatchSizeWidth { get; set; } = default!;
        public decimal BatchSizeHeight { get; set; } = default!;
        public string WidthUnit { get; set; } = default!;
        public string HeightUnit { get; set; } = default!;
        public BatchStatus Status { get; set; }

        public void StartBatching()
        {
            if (Status != BatchStatus.Ready)
                throw new DomainException("Batch đang được sử dụng rồi");
            Status = BatchStatus.InUse;

        }

        public void FinishBatching(string triggerByUserId)
        {
            if (Status != BatchStatus.InUse)
                throw new DomainException("Batch chưa được sử dụng");
            Status = BatchStatus.Cleaning;
        }

        public void CompleteCleaning(string triggerByUserId)
        {
            if (Status != BatchStatus.Cleaning)
                throw new DomainException("Batch chưa được làm sạch");
            Status = BatchStatus.Ready;
        }

        public void SendToMaintenance(string triggerByUserId)
        {
            if (Status != BatchStatus.Ready)
                throw new DomainException("Batch đang được sử dụng, không thể bảo trì");
        }
    }
}