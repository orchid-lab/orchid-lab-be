using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Linkeds : BaseGuidEntity
    {
        public string? TaskID {  get; set; }
        [ForeignKey(nameof(TaskID))]
        public virtual Tasks? Task { get; set; }
        public string? SampleID {  get; set; }
        [ForeignKey(nameof(SampleID))]
        public virtual Samples? Sample { get; set; }
        public string? ExperimentLogID {  get; set; }
        [ForeignKey(nameof(ExperimentLogID))]
        public virtual ExperimentLogs? ExperimentLog { get; set; }
        public string? StageID { get; set; }
        public int ProcessStatus { get; set; }
    }
}
