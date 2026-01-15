using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvent
{
    public record class ExperimentLogStarted(string ExperimentLogId, int MethodId, string TechnicianId) : DomainEvent;
}
