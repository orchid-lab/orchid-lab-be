using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events
{
    public record TaskReportedByTechnicianEvent(
        string TaskId,
        string TechnicianId,
        string ResearcherId) : DomainEvent
    {
    }
}
