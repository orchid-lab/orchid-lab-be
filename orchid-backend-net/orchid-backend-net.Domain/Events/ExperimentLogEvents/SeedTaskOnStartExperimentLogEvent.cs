using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvents
{
    public record SeedTaskOnStartExperimentLogEvent(
        string ExperimentLogId,
        int MethodId,
        string AssignTo,
        string CreatedBy) : DomainEvent;
}
