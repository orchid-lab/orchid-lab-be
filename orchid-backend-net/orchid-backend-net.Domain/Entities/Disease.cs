using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Disease : BaseIntEntity
    {
        public required string Name { get; set; }       
        public required string Code { get; set; }     
        public required string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual List<MonitoringLogs> MonitoringLogs { get; set; } = new();
    }
}