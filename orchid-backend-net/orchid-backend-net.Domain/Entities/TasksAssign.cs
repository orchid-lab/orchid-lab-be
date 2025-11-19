using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TasksAssign : BaseGuidEntity
    {
        public string TechnicianID {  get; set; }
        [ForeignKey(nameof(TechnicianID))]
        public virtual Users Technician { get; set; }
        public string TaskID {  get; set; }
        [ForeignKey(nameof(TaskID))]
        public virtual Tasks Task { get; set; }
        public bool Status { get; set; }
    }
}
