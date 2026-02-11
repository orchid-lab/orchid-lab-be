using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskAcceptedByTechnicianNotification
{
    internal class TaskAcceptedByTechnicianEventHandler
        (INotificationRepository notificationRepository,
        INotificationPushService notificationService,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<DomainEventNotification<TaskAcceptedByTechnicianEvent>>
    {
        public async Task Handle(DomainEventNotification<TaskAcceptedByTechnicianEvent> evt, CancellationToken cancellationToken)
        {
            var technician = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.TechnicianId, cancellationToken) 
                ?? throw new NotFoundException("Không tìm thấy technician này.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task này.");

            var title = "Task đã được nhận";
            var content = $"Task {task.Name} đã được nhận bởi Technician {technician.Name}";

            var notification = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.ResearcherId, title, content);
            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.ResearcherId, notification.Title, notification.Content);
        }
    }
}
