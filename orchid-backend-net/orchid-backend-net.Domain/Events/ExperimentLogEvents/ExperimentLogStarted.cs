using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvents
{
    public record class ExperimentLogStarted(
        string ExperimentLogId, 
        int BatchId, 
        string BatchName,
        string ExperimentName,
        string StartByUserId) : DomainEvent;
}
