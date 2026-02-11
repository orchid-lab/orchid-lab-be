using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvents
{
    public record ExperimentLogCancel(string ExperimentLogId, string? Reason) : DomainEvent;
}
