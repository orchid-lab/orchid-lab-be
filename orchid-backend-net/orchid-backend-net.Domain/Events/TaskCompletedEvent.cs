using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events
{
    public record TaskCompletedEvent(
        string TaskId,
        string ResearcherId,
        string TechnicianId,
        bool IsCompletedInTime) : DomainEvent
    {
    }
}
