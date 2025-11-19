using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Referents : BaseGuidEntity
    {
        public string StageID {  get; set; }
        [ForeignKey(nameof(StageID))]
        public virtual Stages Stage { get; set; }
        public string Name { get; set; }
        public decimal ValueFrom { get; set; }
        public decimal ValueTo { get; set; }
        public string MeasurementUnit { get; set; }
        public bool Status {  get; set; }
        public virtual ICollection<ReportAttributes> ReportAttributes { get; set; }
    }
}
