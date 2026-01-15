using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvent
{
    public record ExperimentLogCompleted(string ExperimentLogId)
        : DomainEvent;
}
