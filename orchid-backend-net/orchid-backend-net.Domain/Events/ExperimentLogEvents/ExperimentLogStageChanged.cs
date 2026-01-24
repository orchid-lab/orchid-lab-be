using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvents
{
    public record ExperimentLogStageChanged(string ExperimentLogId, int CurrentStageOrder, string TechnicianId, string ResearcherId)
        : DomainEvent;
}
