using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskAssignedToTechnicianNotification
{
    internal class TaskAssignedToTechnicianEventHandler(
        INotificationRepository notificationRepository,
        INotificationPushService notificationService,
        IUserRepository userRepository,
        ITaskRepository taskRepository)
        : INotificationHandler<DomainEventNotification<TaskAssignedToTechnicianEvent>>
    {
        public async Task Handle(DomainEventNotification<TaskAssignedToTechnicianEvent> evt, CancellationToken cancellationToken)
        {
            var researcher = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.ResearcherId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy researcher này.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task này.");

            var title = "Task mới được giao";
            var content = $"Task {task.Name} đã được giao bởi Researcher {researcher.Name}";

            var notification = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.TechnicianId, title, content, Domain.Common.Enum.NotificationTargetType.Task, task.ID.ToString());
            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.TechnicianId, notification.Title, notification.Content);
        }
    }
}
