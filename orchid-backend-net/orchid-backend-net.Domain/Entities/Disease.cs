using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Disease : BaseIntEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<MonitoringLogs> MonitoringLogs { get; set; } = [];
    }
}
