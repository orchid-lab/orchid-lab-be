using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskAcceptedByTechnicianNotification
{
    public record TaskAcceptedByTechnicianNotification(TaskAcceptedByTechnicianEvent DomainEvent) : INotification;
    internal class TaskAcceptedByTechnicianEventHandler
        (INotificationRepository notificationRepository,
        IHubnotificationService hubService,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<TaskAcceptedByTechnicianNotification>
    {
        public async Task Handle(TaskAcceptedByTechnicianNotification evt, CancellationToken cancellationToken)
        {
            var technician = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.TechnicianId, cancellationToken) 
                ?? throw new NotFoundException("Không tìm thấy technician này.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task này.");
            var notification = new Domain.Entities.Notification
            {
                UserId = evt.DomainEvent.TechnicianId,
                Title = "Task đã được nhận",
                Content = $"Task {task.Name} đã được nhận bởi Technician {technician.Name}",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.ResearcherId, notification.Title, notification.Content);
        }
    }
}
