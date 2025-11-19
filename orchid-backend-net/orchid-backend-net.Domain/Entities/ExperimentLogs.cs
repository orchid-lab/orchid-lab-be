using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class ExperimentLogs : BaseSoftDelete
    {
        public string Name { get; set; }
        public string CurrentStageID { get; set; }  
        public decimal InfectedRateInReality { get; set; }
        public string MethodID {  get; set; }
        [ForeignKey(nameof(MethodID))]
        public virtual Methods Method {  get; set; }
        public string Description { get; set; }
        public string TissueCultureBatchID {  get; set; }
        [ForeignKey(nameof(TissueCultureBatchID))]
        public virtual TissueCultureBatches TissueCultureBatch {  get; set; }
        public virtual ICollection<Linkeds> Linkeds { get; set; } = [];
        public virtual ICollection<Hybridizations> Hybridizations { get; set; } = [];
        //public enum Status
        public int Status {  get; set; }
    }
}
