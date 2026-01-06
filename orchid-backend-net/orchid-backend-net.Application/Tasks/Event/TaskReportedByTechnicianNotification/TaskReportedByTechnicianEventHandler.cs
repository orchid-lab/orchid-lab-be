using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Events;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskReportedByTechnicianNotification
{
    public record TaskReportedByTechnicianNotification(TaskReportedByTechnicianEvent DomainEvent) : INotification;
    internal class TaskReportedByTechnicianEventHandler(
        IHubnotificationService hubService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<TaskReportedByTechnicianNotification>
    {
        public async Task Handle(TaskReportedByTechnicianNotification evt, CancellationToken cancellationToken)
        {
            var technician = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.TechnicianId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task.");

            Notification noti = new()
            {
                UserId = evt.DomainEvent.ResearcherId,
                Title = "Task được yêu cầu xét duyệt",
                Content = $"Task {task.Name} được yêu cầu xét duyệt bởi {technician.Name}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
            };
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.ResearcherId, noti.Title, noti.Content);
        }
    }
}
