using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Hybridizations : BaseGuidEntity
    {
        public string ParentID {  get; set; }
        [ForeignKey(nameof(ParentID))]
        public virtual Seedlings Parent { get; set; }
        public string ExperimentLogID {  get; set; }
        [ForeignKey(nameof(ExperimentLogID))]
        public virtual ExperimentLogs ExperimentLog { get; set; }
        public bool Status {  get; set; }
    }
}
