using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.BatchEvents;
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


        /// <summary>
        /// use this method to start using the batch
        /// only triggered by experiment logs
        /// </summary>
        /// <exception cref="DomainException"></exception>
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
            var oldStatus = Status;
            Status = BatchStatus.Cleaning;
            AddDomainEvent(new BatchStatusChangedEvent(ID, oldStatus, Status, triggerByUserId));
        }

        public void CompleteCleaning(string triggerByUserId)
        {
            if (Status != BatchStatus.Cleaning)
                throw new DomainException("Batch chưa được làm sạch");
            var oldStatus = Status;
            Status = BatchStatus.Ready;
            AddDomainEvent(new BatchStatusChangedEvent(ID, oldStatus, Status, triggerByUserId));
        }

        public void SetToMaintenance(string triggerByUserId)
        {
            if (Status != BatchStatus.Ready)
                throw new DomainException("Batch đang được sử dụng, không thể bảo trì");
            var oldStatus = Status;
            Status = BatchStatus.Maintenance;
            AddDomainEvent(new BatchStatusChangedEvent(ID, oldStatus, Status, triggerByUserId));
        }

        public void SetToInactive(string userTriggerId)
        {
            if (Status == BatchStatus.InUse || Status == BatchStatus.Cleaning)
                throw new DomainException("Không thể chuyển batch sang Inactive khi đang sử dụng/làm sạch.");
            var oldStatus = Status;
            Status = BatchStatus.Inactive;
            AddDomainEvent(new BatchStatusChangedEvent(ID, oldStatus, Status, userTriggerId));
        }
    }
}