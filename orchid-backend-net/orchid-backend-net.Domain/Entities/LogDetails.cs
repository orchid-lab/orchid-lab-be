using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class LogDetails : BaseGuidEntity
    {
        public string MonitoringLogsId { get; set; }
        [ForeignKey(nameof(MonitoringLogsId))]
        public virtual MonitoringLogs MonitoringLogs { get; set; }
        public required string SampleStageRequirementId { get; set; }
        [ForeignKey(nameof(SampleStageRequirementId))]
        public virtual required SampleStageRequirement SampleStageRequirement { get; set; }
        public required decimal MeasuredValue { get; set; }
        public required bool IsMatch { get; set; }
    }
}
