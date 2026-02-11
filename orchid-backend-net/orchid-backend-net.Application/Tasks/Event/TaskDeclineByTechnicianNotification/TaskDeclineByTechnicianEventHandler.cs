using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskDeclineByTechnicianNotification
{
    internal class TaskDeclineByTechnicianEventHandler(
        INotificationPushService notificationService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<DomainEventNotification<TaskDeclineByTechnicianEvent>>
    {
        public async Task Handle(DomainEventNotification<TaskDeclineByTechnicianEvent> evt, CancellationToken cancellationToken)
        {
            var technician = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.TechnicianId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task.");

            var title = "Task đã bị từ chối";
            var content = !string.IsNullOrWhiteSpace(evt.DomainEvent.Reason) 
                ? $"Task {task.Name} đã bị từ chối bởi Technician {technician.Name} với lý do {evt.DomainEvent.Reason}." 
                : $"Task {task.Name} đã bị từ chối bởi Technician {technician.Name}.";

            Domain.Entities.Notification noti = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.ResearcherId, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.ResearcherId, noti.Title, noti.Content);
        }
    }
}
