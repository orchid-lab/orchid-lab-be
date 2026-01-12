using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class MonitoringLogs : BaseGuidEntity
    {
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }
        public string? AnalyticResultId { get; set; }
        [ForeignKey(nameof(AnalyticResultId))]
        public virtual AnalyticResults? AnalyticResult { get; set; }
        public required string SampleStageId { get; set; }
        [ForeignKey(nameof(SampleStageId))]
        public virtual SampleStage SampleStage { get; set; }
        public int? DiseaseId { get; set; }
        [ForeignKey(nameof(DiseaseId))]
        public virtual Disease? Disease { get; set; }
        public string? Notes { get; set; }
        public required MonitoringLogStatus Status { get; set; }
        //0 - Đang chờ duyệt
        //1 - Đã duyệt
        public DateOnly? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateOnly? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateOnly? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public virtual List<LogDetails> LogDetails { get; set; } = [];
    }
}