using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskCancelledNotification
{
    internal class TaskCancelledNotification(
        INotificationRepository notificationRepository,
        INotificationPushService notificationService,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<DomainEventNotification<TaskCancelledEvent>>
    {
        public async Task Handle(DomainEventNotification<TaskCancelledEvent> notification, CancellationToken cancellationToken)
        {
            var researcher = await userRepository.GetByIdAsync(notification.DomainEvent.CancelledBy, cancellationToken);
            var task = await taskRepository.FindAsync(t => t.ID == notification.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không thấy task này");

            var title = $"Task {task.Name} đã bị hủy";
            var content = $"Task {task.Name} đã bị hủy bởi {researcher.Name}. Lý do: {notification.DomainEvent.Reason ?? "Không có lý do nào được cung cấp."}";

            var noti = CreateNotificationHelper.CreateForSingleUsers(notification.DomainEvent.Technician, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(notification.DomainEvent.Technician, noti.Title, noti.Content);
        }
    }
}
