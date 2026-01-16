using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvent
{
    public record ExperimentLogDestroyed(
        string ExperimentLogId,
        string? Reason)
        : DomainEvent;
}
