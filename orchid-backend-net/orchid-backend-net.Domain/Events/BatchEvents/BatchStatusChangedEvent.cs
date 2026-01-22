using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.BatchEvents
{
    public record BatchStatusChangedEvent(
        int BatchId,
        BatchStatus OldStatus,
        BatchStatus NewStatus,
        string? TriggeredByUserId) : DomainEvent;
}
