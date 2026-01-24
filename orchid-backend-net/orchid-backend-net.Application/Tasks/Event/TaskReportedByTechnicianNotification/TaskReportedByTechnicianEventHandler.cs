using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskReportedByTechnicianNotification
{
    public record TaskReportedByTechnicianNotification(TaskReportedByTechnicianEvent DomainEvent) : INotification;
    internal class TaskReportedByTechnicianEventHandler(
        INotificationPushService notificationService,
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

            var title = "Task được yêu cầu xét duyệt";
            var content = $"Task {task.Name} được yêu cầu xét duyệt bởi {technician.Name}";

            Domain.Entities.Notification noti = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.ResearcherId, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.ResearcherId, noti.Title, noti.Content);
        }
    }
}
