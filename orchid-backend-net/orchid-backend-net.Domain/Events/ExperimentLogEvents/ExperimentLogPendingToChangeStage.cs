using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvents
{
    public record ExperimentLogPendingToChangeStage(
        string ExperimentLogId,
        int CurrentStageOrder,
        string AssignTo)
        : DomainEvent;
}
