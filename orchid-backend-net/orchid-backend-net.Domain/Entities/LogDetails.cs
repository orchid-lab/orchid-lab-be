using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class LogDetails : BaseGuidEntity
    {
        public string MonitoringLogsId { get; set; }
        [ForeignKey(nameof(MonitoringLogsId))]
        public virtual MonitoringLogs MonitoringLogs { get; set; }
        public string StageRequirementDefinitionId { get; set; }
        [ForeignKey(nameof(StageRequirementDefinitionId))]
        public virtual StageRequirementDefinition StageRequirementDefinition { get; set; }
        public required decimal MeasuredValue { get; set; }
        public required bool IsMatch { get; set; }
    }
}
