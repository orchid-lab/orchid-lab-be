using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.MonitoringLogEvents
{
    /// <summary>
    /// Raised when technician submits monitoring log for researcher approval.
    /// Used for both initial submission (WaitingForApproval) and resubmission (Revised).
    /// </summary>
    public record MonitoringLogSubmittedForApprovalEvent(
        string MonitoringLogId,
        string TechnicianId,
        string ResearcherId,
        bool IsResubmission) : DomainEvent;
}