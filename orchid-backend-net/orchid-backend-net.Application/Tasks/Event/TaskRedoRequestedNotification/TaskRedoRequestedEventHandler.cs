using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Events;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Tasks.Event.TaskRedoRequestedNotification
{
    public record TaskRedoRequestedNotification(TaskRedoRequestedEvent DomainEvent) : INotification;
    internal class TaskRedoRequestedEventHandler(
        IHubnotificationService hubService,
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

            Notification noti = new()
            {
                UserId = evt.DomainEvent.TechnicianId,
                Title = "Task đã yêu cầu làm lại",
                Content = $"Task {task.Name} đã được yêu cầu làm lại bởi Researcher {researcher.Name}.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
            };

            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await hubService.PushToUserAsync(evt.DomainEvent.TechnicianId, noti.Title, noti.Content);
        }
    }
}
