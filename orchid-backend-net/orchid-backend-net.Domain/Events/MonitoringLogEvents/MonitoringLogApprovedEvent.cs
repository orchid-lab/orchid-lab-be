using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.MonitoringLogEvents
{
    /// <summary>
    /// Raised when researcher approves monitoring log.
    /// Triggers notification to technician and marks log as newest approved.
    /// </summary>
    public record MonitoringLogApprovedEvent(
        string MonitoringLogId,
        string ResearcherId,
        string TechnicianId) : DomainEvent;
}