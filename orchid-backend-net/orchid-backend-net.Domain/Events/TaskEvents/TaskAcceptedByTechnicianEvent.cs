using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.TaskEvents
{
    public record TaskAcceptedByTechnicianEvent(
        string TaskId,
        string TechnicianId,
        string ResearcherId
        ) : DomainEvent
    {
    }
}
