using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Stages : BaseGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Step { get; set; } // Assuming Step is an integer representing the order of the stage
        public string MethodID {  get; set; }
        [ForeignKey(nameof(MethodID))]
        public virtual Methods Method { get; set; }
        public int DateOfProcessing { get; set; }
        public bool Status {  get; set; }
        public virtual ICollection<ElementInStage> ElementInStages { get; set; }
        public virtual ICollection<TaskTemplates> TaskTemplates { get; set; }
    }
}
