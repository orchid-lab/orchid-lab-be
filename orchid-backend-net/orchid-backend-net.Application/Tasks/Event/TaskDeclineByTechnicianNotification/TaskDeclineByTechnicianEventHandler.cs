using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskDeclineByTechnicianNotification
{
    public record TaskDeclineByTechnicianNotification(TaskDeclineByTechnicianEvent DomainEvent) : INotification;
    internal class TaskDeclineByTechnicianEventHandler(
        IHubnotificationService hubService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<TaskDeclineByTechnicianNotification>
    {
        public async Task Handle(TaskDeclineByTechnicianNotification evt, CancellationToken cancellationToken)
        {
            var technician = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.TechnicianId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task.");

            Domain.Entities.Notification noti = new()
            {
                UserId = evt.DomainEvent.ResearcherId,
                Title = "Task đã bị từ chối",
                Content = $"Task {task.Name} đã bị từ chối bởi Technician {technician.Name}.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
            };
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.ResearcherId, noti.Title, noti.Content);
        }
    }
}
