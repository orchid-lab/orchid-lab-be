using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class LogDetails : BaseGuidEntity
    {
        public required string RequirementId { get; set; }
        public virtual MethodStageSampleRequirement Requirement { get; set; }
        public string MonitoringLogsId { get; set; }
        [ForeignKey(nameof(MonitoringLogsId))]
        public virtual MonitoringLogs MonitoringLogs { get; set; }
        public required decimal MeasuredValue { get; set; }
        public required bool IsMatch { get; set; }
    }
}
