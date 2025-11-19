using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TissueCultureBatches : BaseGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LabRoomID {  get; set; }
        [ForeignKey(nameof(LabRoomID))]
        public virtual LabRooms LabRoom { get; set; }
        public bool Status {  get; set; }
        public virtual ICollection<ExperimentLogs> ExperimentLogs { get; set; } = [];
    }
}
