using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskTemplates : BaseGuidEntity
    {
        public string Name { get; set; }
        public string StageID { get; set; }
        [ForeignKey(nameof(StageID))]
        public virtual Stages Stage { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<TaskTemplateDetails> TemplateDetails { get; set; }
        //public virtual Stages Stages { get; set; }
    }
}
