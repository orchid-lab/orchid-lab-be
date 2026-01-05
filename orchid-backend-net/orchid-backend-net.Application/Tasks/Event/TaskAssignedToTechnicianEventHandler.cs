using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Event.Notification;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event
{
    public class TaskAssignedToTechnicianEventHandler(
        INotificationRepository notificationRepository,
        IHubnotificationService hubService)
        : INotificationHandler<TaskAssignedNotification>
    {
        public async Task Handle(TaskAssignedNotification evt, CancellationToken cancellationToken)
        {
            var notification = new Domain.Entities.Notification
            {
                UserId = evt.DomainEvent.TechnicianId,
                Title = "Task mới được giao",
                Content = "Task đã được giao bởi Researcher",
                CreatedAt = DateTime.UtcNow
            };
            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.TechnicianId, notification.Title, notification.Content);
        }
    }
}
