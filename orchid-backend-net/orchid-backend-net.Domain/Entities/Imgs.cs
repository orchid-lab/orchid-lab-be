using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Imgs : BaseGuidEntity
    {
        public string MonitoringLogsId { get; set; }
        [ForeignKey(nameof(MonitoringLogsId))]
        public virtual MonitoringLogs MonitoringLogs { get; set; }
        public required string Url { get; set; }
    }
}