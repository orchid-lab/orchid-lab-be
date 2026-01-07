using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Methods : BaseIntEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<ExperimentLogs> ExperimentLogs { get; set; } = [];
        public virtual IEnumerable<MethodStages> Stages { get; set; } = [];
    }
}