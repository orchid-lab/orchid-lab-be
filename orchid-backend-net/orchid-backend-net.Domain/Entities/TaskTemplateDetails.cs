using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskTemplateDetails : BaseGuidEntity
    {
        public string TaskTemplateID { get; set; }
        [ForeignKey(nameof(TaskTemplateID))]
        public virtual TaskTemplates TaskTemplate { get; set; }
        public string Element { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal ExpectedValue { get; set; }
        public string Unit {  get; set; }
        public bool IsRequired { get; set; }
        public bool Status { get; set; }
    }
}
