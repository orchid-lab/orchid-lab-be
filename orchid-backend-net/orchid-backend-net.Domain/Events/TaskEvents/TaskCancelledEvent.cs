using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.TaskEvents
{
    public record TaskCancelledEvent(string TaskId, string CancelledBy, string Technician,string? Reason)
        : DomainEvent;
}
