using MediatR;
using orchid_backend_net.Domain.Events;

namespace orchid_backend_net.Application.Tasks.Event.Notification
{
    public record TaskAssignedNotification(TaskAssignedToTechnicianEvent DomainEvent) : INotification;
}
