using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskCompletedByTechnicianNotification
{
    public record TaskCompletedNotification(TaskCompletedEvent DomainEvent) : INotification;
    internal class TaskCompletedEventHandler(
        IHubnotificationService hubService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<TaskCompletedNotification>
    {
        public async Task Handle(TaskCompletedNotification evt, CancellationToken cancellationToken)
        {
            var researcher = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.ResearcherId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task.");
            Domain.Entities.Notification noti = new()
            {
                UserId = evt.DomainEvent.TechnicianId,
                Title = "Task đã hoàn thành",
                Content = $"Task {task.Name} đã được duyệt hoàn thành bởi {researcher.Name}",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.TechnicianId, noti.Title, noti.Content);
        }
    }
}
