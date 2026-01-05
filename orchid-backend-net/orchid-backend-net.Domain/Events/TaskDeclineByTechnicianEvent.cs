using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events
{
    public record TaskDeclineByTechnicianEvent(
        string TaskId,
        string TechnicianId,
        string ResearcherId,
        string? Reason
        ) : DomainEvent
    {
    }
}
