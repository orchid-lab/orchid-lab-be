using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Methods : BaseGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Status {  get; set; }
        public virtual ICollection<Stages> Stages { get; set; } = [];
        public virtual ICollection<ExperimentLogs> ExperimentLogs { get; set; } = [];
    }
}
