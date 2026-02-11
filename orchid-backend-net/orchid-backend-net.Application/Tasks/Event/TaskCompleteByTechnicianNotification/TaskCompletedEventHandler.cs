using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.TaskEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.Event.TaskCompletedByTechnicianNotification
{
    internal class TaskCompletedEventHandler(
        INotificationPushService notificationService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository) : INotificationHandler<DomainEventNotification<TaskCompletedEvent>>
    {
        public async Task Handle(DomainEventNotification<TaskCompletedEvent> evt, CancellationToken cancellationToken)
        {
            var researcher = await userRepository.FindAsync(u => u.ID == evt.DomainEvent.ResearcherId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician.");
            var task = await taskRepository.FindAsync(t => t.ID == evt.DomainEvent.TaskId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task.");

            var title = "Task đã hoàn thành";
            var content = $"Task {task.Name} đã được duyệt hoàn thành bởi {researcher.Name}";

            Domain.Entities.Notification noti = CreateNotificationHelper.CreateForSingleUsers(evt.DomainEvent.TechnicianId, title, content);

            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await notificationService.PushToSingleUserAsync(evt.DomainEvent.TechnicianId, noti.Title, noti.Content);
        }
    }
}
