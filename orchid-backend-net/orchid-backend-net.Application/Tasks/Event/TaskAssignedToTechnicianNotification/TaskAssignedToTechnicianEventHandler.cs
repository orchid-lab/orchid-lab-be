using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskAssignedToTechnicianNotification
{
    public record TaskAssignedNotification(TaskAssignedToTechnicianEvent DomainEvent) : INotification;
    internal class TaskAssignedToTechnicianEventHandler(
        INotificationRepository notificationRepository,
        INotificationPushService notificationService,
        IUserRepository userRepository,
        ITaskRepository taskRepository)
        : INotificationHandler<TaskAssignedNotification>
    {
        public async Task Handle(TaskAssignedNotification evt, CancellationToken cancellationToken)
        {
            var researcher = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.ResearcherId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy researcher này.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task này.");

            var title = "Task mới được giao";
            var content = $"Task {task.Name} đã được giao bởi Researcher {researcher.Name}";

            var notification = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.TechnicianId, title, content);

            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.TechnicianId, notification.Title, notification.Content);
        }
    }
}
