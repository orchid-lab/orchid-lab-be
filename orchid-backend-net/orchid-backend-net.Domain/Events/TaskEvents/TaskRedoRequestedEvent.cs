using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.TaskEvents
{
    public record TaskRedoRequestedEvent(
        string TaskId,
        string ResearcherId,
        string TechnicianId,
        string? Reason) : DomainEvent
    {
    }
}
