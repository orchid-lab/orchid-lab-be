using System.ComponentModel.DataAnnotations.Schema;
using orchid_backend_net.Domain.Common;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class DiseaseIncident : AuditableEntity
    {
        public required string SampleStageId { get; set; }
        [ForeignKey(nameof(SampleStageId))]
        public virtual SampleStage SampleStage { get; set; } = null!;

        public required string MonitoringLogId { get; set; }
        [ForeignKey(nameof(MonitoringLogId))]
        public virtual MonitoringLogs MonitoringLog { get; set; } = null!;

        public required int DiseaseId { get; set; }
        [ForeignKey(nameof(DiseaseId))]
        public virtual Disease Disease { get; set; } = null!;

        // Confidence score from AI (0.0 - 1.0)
        public decimal AIConfidence { get; set; }

        public DiseaseIncidentStatus Status { get; set; }

        // Researcher/Technician điền vào sau khi kiểm tra thực tế
        public string? ReviewNote { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public virtual List<DiseaseIncidentAction> Actions { get; set; } = new();

        // Domain methods
        public void ConfirmByHuman(string reviewerId, string? note)
        {
            if (Status != DiseaseIncidentStatus.AIDetected && Status != DiseaseIncidentStatus.UnderReview)
                throw new DomainException("Chỉ có thể xác nhận sự cố đang chờ review.");
            Status = DiseaseIncidentStatus.Confirmed;
            ReviewedBy = reviewerId;
            ReviewedAt = DateTime.UtcNow;
            ReviewNote = note;
        }

        public void DismissByHuman(string reviewerId, string reason)
        {
            if (Status != DiseaseIncidentStatus.AIDetected && Status != DiseaseIncidentStatus.UnderReview)
                throw new DomainException("Chỉ có thể dismiss sự cố đang chờ review.");
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Phải có lý do khi dismiss.");
            Status = DiseaseIncidentStatus.Dismissed;
            ReviewedBy = reviewerId;
            ReviewedAt = DateTime.UtcNow;
            ReviewNote = reason;
        }

        public void AddAction(string actionDescription, string performedBy)
        {
            if (Status != DiseaseIncidentStatus.Confirmed)
                throw new DomainException("Chỉ có thể thêm hành động cho sự cố đã xác nhận.");
            Actions.Add(new DiseaseIncidentAction
            {
                DiseaseIncidentId = ID,
                ActionDescription = actionDescription,
                PerformedBy = performedBy,
                PerformedAt = DateTime.UtcNow
            });
        }
    }
}
