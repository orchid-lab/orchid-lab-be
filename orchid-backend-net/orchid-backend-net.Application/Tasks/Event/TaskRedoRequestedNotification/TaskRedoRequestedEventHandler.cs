using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskRedoRequestedNotification
{
    public record TaskRedoRequestedNotification(TaskRedoRequestedEvent DomainEvent) : INotification;
    internal class TaskRedoRequestedEventHandler(
        INotificationPushService notificationService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<TaskRedoRequestedNotification>
    {
        public async Task Handle(TaskRedoRequestedNotification evt, CancellationToken cancellationToken)
        {
            var researcher = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.ResearcherId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy researcher.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task.");

            var title = "Task đã yêu cầu làm lại";
            var content = $"Task {task.Name} đã được yêu cầu làm lại bởi Researcher {researcher.Name}.";

            Domain.Entities.Notification noti = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.TechnicianId, title, content);

            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.TechnicianId, noti.Title, noti.Content);
        }
    }
}
