using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class MonitoringLogs : AuditableEntity
    {
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }
        //after analytic done
        public string? AnalyticResultId { get; set; }
        [ForeignKey(nameof(AnalyticResultId))]
        public virtual AnalyticResults? AnalyticResult { get; set; }
        //Input in api
        public required string SampleStageId { get; set; }
        [ForeignKey(nameof(SampleStageId))]
        public virtual SampleStage SampleStage { get; set; }
        //after analytic done
        public int? DiseaseId { get; set; }
        [ForeignKey(nameof(DiseaseId))]
        public virtual Disease? Disease { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public MonitoringLogStatus Status { get; set; }
        //0 - Đang chờ duyệt
        //1 - Đã duyệt
        public DateOnly? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public virtual List<LogDetails> LogDetails { get; set; } = new();
        public bool IsNewest { get; set; }

        public void Created()
        {
            Status = MonitoringLogStatus.Created;
        }

        public void WaitingForApproval()
        {
            Status = MonitoringLogStatus.WaitingForApproval;
        }

        public void Approved()
        {
            Status = MonitoringLogStatus.Approved;
        }

        public void AddLogDetails(string stageRequirementDefinitionId, decimal measuredValue, bool isMatch)
        {
            LogDetails.Add(new LogDetails
            {
                MonitoringLogsId = ID,
                StageRequirementDefinitionId = stageRequirementDefinitionId,
                MeasuredValue = measuredValue,
                IsMatch = isMatch
            });
        }
    }
}