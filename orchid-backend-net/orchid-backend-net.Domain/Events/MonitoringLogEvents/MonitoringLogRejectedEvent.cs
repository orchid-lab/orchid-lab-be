using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.MonitoringLogEvents
{
    /// <summary>
    /// Raised when researcher rejects monitoring log and requests revision.
    /// Triggers notification to technician with rejection reason.
    /// </summary>
    public record MonitoringLogRejectedEvent(
        string MonitoringLogId,
        string ResearcherId,
        string TechnicianId,
        string RejectionReason) : DomainEvent;
}