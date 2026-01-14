using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.TaskEvents
{
    public record TaskAssignedToTechnicianEvent(
        string TaskId,
        string TechnicianId,
        string ResearcherId
        ) : DomainEvent
    {
    }
}
