using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvent
{
    public record ExperimentLogStageChanged(string ExperimentLogId, int CurrentStageOrder, string TechinicianId)
        : DomainEvent;
}
