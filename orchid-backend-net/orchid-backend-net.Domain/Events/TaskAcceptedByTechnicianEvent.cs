using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events
{
    public record TaskAcceptedByTechnicianEvent(
        string TaskId,
        string TechnicianId,
        string ResearcherId
        ) : DomainEvent
    {
    }
}
