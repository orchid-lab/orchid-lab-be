using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class MonitoringLogs : BaseGuidEntity
    {
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users User { get; set; }
        public required string AnalyticResultId { get; set; }
        [ForeignKey(nameof(AnalyticResultId))]
        public virtual AnalyticResults AnalyticResult { get; set; }
        public required string SampleId { get; set; }
        [ForeignKey(nameof(SampleId))]
        public virtual Samples Sample { get; set; }
        public string? Notes { get; set; }
        public required int SampleStageOrder { get; set; }
        public required string Status { get; set; }
        //0 - Đang chờ duyệt
        //1 - Đã duyệt
        public DateOnly? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateOnly? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateOnly? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public virtual IEnumerable<LogDetails> LogDetails { get; set; } = [];
    }
}