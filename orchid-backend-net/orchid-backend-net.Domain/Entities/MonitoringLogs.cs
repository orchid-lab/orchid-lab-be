using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using orchid_backend_net.Domain.Events.MonitoringLogEvents;
using orchid_backend_net.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    /// <summary>
    /// Aggregate root for monitoring log workflow.
    /// Manages lifecycle: Created → WaitingForApproval/Revised → Approved/Rejected.
    /// <ul>
    /// <li>Technician creates and submits monitoring logs</li>
    /// <li>Researcher reviews and approves/rejects</li>
    /// <li>Only one approved log per sample stage can have IsNewest = true</li>
    /// </ul>
    /// </summary>
    public class MonitoringLogs : AuditableEntity
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
        
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public MonitoringLogStatus Status { get; set; }
        
        // Rejection tracking
        public string? RejectionReason { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string? RejectedBy { get; set; }
        
        public DateOnly? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public virtual List<LogDetails> LogDetails { get; set; } = new();
        
        /// <summary>
        /// Indicates if this is the newest approved monitoring log for the sample stage.
        /// Only one approved log per sample stage should have IsNewest = true.
        /// </summary>
        public bool IsNewest { get; set; }

        // ===== DOMAIN METHODS =====

        /// <summary>
        /// Initializes monitoring log with Created status.
        /// Called immediately after entity construction.
        /// </summary>
        public void Created()
        {
            Status = MonitoringLogStatus.Created;
        }

        /// <summary>
        /// Technician submits monitoring log for researcher approval.
        /// Transitions from Created → WaitingForApproval or Rejected → Revised.
        /// </summary>
        /// <param name="researcherId">ID of researcher who owns the experiment log</param>
        /// <exception cref="DomainException">When status is not Created or Rejected</exception>
        public void SubmitForApproval(string researcherId)
        {
            if (Status != MonitoringLogStatus.Created 
                && Status != MonitoringLogStatus.Rejected)
                throw new DomainException("Chỉ có thể gửi báo cáo ở trạng thái 'Đã tạo' hoặc 'Bị từ chối'.");

            bool isResubmission = Status == MonitoringLogStatus.Rejected;
            
            // Set status based on whether this is first submission or resubmission
            Status = isResubmission 
                ? MonitoringLogStatus.Revised 
                : MonitoringLogStatus.WaitingForApproval;
            
            // Clear rejection information on resubmission
            if (isResubmission)
            {
                RejectionReason = null;
                RejectedDate = null;
                RejectedBy = null;
            }

            AddDomainEvent(new MonitoringLogSubmittedForApprovalEvent(
                ID,
                UserId,
                researcherId,
                isResubmission));
        }

        /// <summary>
        /// Researcher approves the monitoring log.
        /// Transitions from WaitingForApproval/Revised → Approved.
        /// Sets IsNewest = true (caller must set others to false).
        /// </summary>
        /// <param name="researcherId">ID of researcher approving</param>
        /// <exception cref="DomainException">When status is not WaitingForApproval or Revised</exception>
        public void Approve(string researcherId)
        {
            if (Status != MonitoringLogStatus.WaitingForApproval 
                && Status != MonitoringLogStatus.Revised)
                throw new DomainException("Chỉ có thể duyệt báo cáo đang chờ duyệt hoặc đã chỉnh sửa.");

            Status = MonitoringLogStatus.Approved;
            IsNewest = true; // This becomes the newest approved log

            AddDomainEvent(new MonitoringLogApprovedEvent(
                ID,
                researcherId,
                UserId));
        }

        /// <summary>
        /// Researcher rejects the monitoring log and requests revision.
        /// Transitions from WaitingForApproval/Revised → Rejected.
        /// </summary>
        /// <param name="researcherId">ID of researcher rejecting</param>
        /// <param name="reason">Detailed reason for rejection</param>
        /// <exception cref="DomainException">When status is not WaitingForApproval/Revised or reason is empty</exception>
        public void Reject(string researcherId, string reason)
        {
            if (Status != MonitoringLogStatus.WaitingForApproval 
                && Status != MonitoringLogStatus.Revised)
                throw new DomainException("Chỉ có thể từ chối báo cáo đang chờ duyệt hoặc đã chỉnh sửa.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Lý do từ chối không được để trống.");

            Status = MonitoringLogStatus.Rejected;
            RejectedBy = researcherId;
            RejectionReason = reason;
            RejectedDate = DateTime.UtcNow;

            AddDomainEvent(new MonitoringLogRejectedEvent(
                ID,
                researcherId,
                UserId,
                reason));
        }

        /// <summary>
        /// Technician updates measured values in log details for rejected reports.
        /// Can only be called when status is Rejected.
        /// </summary>
        /// <param name="logDetailId">ID of log detail to update</param>
        /// <param name="newMeasuredValue">New measured value</param>
        /// <param name="minValue">Min acceptable value from stage requirement (nullable)</param>
        /// <param name="maxValue">Max acceptable value from stage requirement (nullable)</param>
        /// <exception cref="DomainException">When status is not Rejected</exception>
        /// <exception cref="NotFoundException">When log detail not found</exception>
        public void UpdateLogDetail(string logDetailId, decimal newMeasuredValue, decimal? minValue, decimal? maxValue)
        {
            if (Status != MonitoringLogStatus.Rejected)
                throw new DomainException("Chỉ có thể cập nhật báo cáo đã bị từ chối.");

            var logDetail = LogDetails.FirstOrDefault(ld => ld.ID == logDetailId)
                ?? throw new NotFoundException("Không tìm thấy log detail.");

            // Calculate isMatch: true if value is within range (or no range defined)
            bool isMatch = true;

            if (minValue.HasValue && newMeasuredValue < minValue.Value)
                isMatch = false;

            if (maxValue.HasValue && newMeasuredValue > maxValue.Value)
                isMatch = false;

            logDetail.MeasuredValue = newMeasuredValue;
            logDetail.IsMatch = isMatch;
        }

        /// <summary>
        /// Adds a new log detail entry during monitoring log creation.
        /// </summary>
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

        /// <summary>
        /// Adds a new log detail entry with range validation using Value Object.
        /// Recommended approach for better encapsulation.
        /// </summary>
        /// <param name="stageRequirementDefinitionId">Stage requirement definition ID</param>
        /// <param name="measuredValue">Measured value</param>
        /// <param name="range">Acceptable measurement range</param>
        public void AddLogDetailsWithRange(
            string stageRequirementDefinitionId,
            decimal measuredValue,
            MeasurementRange range)
        {
            bool isMatch = range.IsValueInRange(measuredValue);
            
            LogDetails.Add(new LogDetails
            {
                MonitoringLogsId = ID,
                StageRequirementDefinitionId = stageRequirementDefinitionId,
                MeasuredValue = measuredValue,
                IsMatch = isMatch
            });
        }

        /// <summary>
        /// Updates log detail with range validation using Value Object.
        /// Can only be called when status is Rejected.
        /// </summary>
        /// <param name="logDetailId">ID of log detail to update</param>
        /// <param name="newMeasuredValue">New measured value</param>
        /// <param name="range">Acceptable measurement range</param>
        /// <exception cref="DomainException">When status is not Rejected</exception>
        /// <exception cref="NotFoundException">When log detail not found</exception>
        public void UpdateLogDetailWithRange(
            string logDetailId,
            decimal newMeasuredValue,
            MeasurementRange range)
        {
            if (Status != MonitoringLogStatus.Rejected)
                throw new DomainException("Chỉ có thể cập nhật báo cáo đã bị từ chối.");

            var logDetail = LogDetails.FirstOrDefault(ld => ld.ID == logDetailId)
                ?? throw new NotFoundException("Không tìm thấy log detail.");

            bool isMatch = range.IsValueInRange(newMeasuredValue);
            
            logDetail.MeasuredValue = newMeasuredValue;
            logDetail.IsMatch = isMatch;
        }
    }
}