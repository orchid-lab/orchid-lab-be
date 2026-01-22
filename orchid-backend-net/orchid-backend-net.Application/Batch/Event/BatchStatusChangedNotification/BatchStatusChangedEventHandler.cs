using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.BatchEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.Event.BatchStatusChangedNotification
{
    public record BatchStatusChangedNotification(BatchStatusChangedEvent DomainEvent) : INotification;
    internal class BatchStatusChangedEventHandler(
        INotificationRepository notificationRepository,
        IHubnotificationService hubService,
        IUserRepository userRepository,
        IBatchesRepository batchRepository) : INotificationHandler<BatchStatusChangedNotification>
    {
        public async Task Handle(BatchStatusChangedNotification evt, CancellationToken cancellationToken)
        {
            var batch = await batchRepository.FindAsync(b => b.ID == evt.DomainEvent.BatchId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy batch này");
            var researchers = await userRepository.FindAllAsync(u => u.RoleID == 2, cancellationToken);

            if (researchers.Any())
                throw new NotFoundException("Không tìm thấy researcher nào");

            List<Domain.Entities.Notification> notifications = new List<Domain.Entities.Notification>();

            var title = $"Batch {batch.BatchName} thay đổi trạng thái";

            var oldStatus = evt.DomainEvent.OldStatus.ToDisplayText();
            var newStatus = evt.DomainEvent.NewStatus.ToDisplayText();
            var content = $"Trạng thái của batch {batch.BatchName} đã thay đổi từ {oldStatus} sang {newStatus}";

            foreach (var researcher in researchers)
            {
                var noti = new Domain.Entities.Notification
                {
                    UserId = researcher.ID,
                    Title = title,
                    Content = content,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };
                await hubService.PushToUserAsync(researcher.ID, noti.Title, noti.Content);
                notifications.Add(noti);
            }
            notificationRepository.AddRange(notifications);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
