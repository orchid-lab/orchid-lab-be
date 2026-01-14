using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskAssignedToTechnicianNotification
{
    public record TaskAssignedNotification(TaskAssignedToTechnicianEvent DomainEvent) : INotification;
    internal class TaskAssignedToTechnicianEventHandler(
        INotificationRepository notificationRepository,
        IHubnotificationService hubService,
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
            var notification = new Domain.Entities.Notification
            {
                UserId = evt.DomainEvent.TechnicianId,
                Title = "Task mới được giao",
                Content = $"Task {task.Name} đã được giao bởi Researcher {researcher.Name}",
                CreatedAt = DateTime.UtcNow,
                IsRead = false  
            };
            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.TechnicianId, notification.Title, notification.Content);
        }
    }
}
