using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskAttributes : BaseGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskID {  get; set; }
        [ForeignKey(nameof(TaskID))]
        public virtual Tasks Task {  get; set; }
        public double Value {  get; set; }
        public string MeasurementUnit { get; set; }
        public bool Status {  get; set; }
    }
}
