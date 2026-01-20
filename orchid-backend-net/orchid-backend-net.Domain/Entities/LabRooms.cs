using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class LabRooms : BaseIntEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public LabRoomStatus Status { get; set; }
        public virtual List<Batches> Batches { get; set; } = new();

        public void ActivateRoom()
        {
            Status = LabRoomStatus.Active;
        }

        public void SetToMaintenance()
        {
            Status = LabRoomStatus.Maintainance;
        }

        public void DeactivateRoom()
        {
            Status = LabRoomStatus.Inactive;
        }
    }
}