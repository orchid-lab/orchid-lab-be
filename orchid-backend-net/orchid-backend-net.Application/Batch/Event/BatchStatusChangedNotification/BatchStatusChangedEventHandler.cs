using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.BatchEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.Event.BatchStatusChangedNotification
{
    internal class BatchStatusChangedEventHandler(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IBatchesRepository batchRepository,
        INotificationPushService notificationService)
        : INotificationHandler<DomainEventNotification<BatchStatusChangedEvent>>
    {
        public async Task Handle(DomainEventNotification<BatchStatusChangedEvent> evt, CancellationToken cancellationToken)
        {
            var batch = await batchRepository.GetByIdAsync(evt.DomainEvent.BatchId, cancellationToken);

            var researchers = await userRepository.FindAllAsync(u => u.RoleID == 2, cancellationToken);
            if (researchers.Count == 0)
                throw new NotFoundException("Không tìm thấy researcher nào");


            //determine title and conent for notification
            var title = $"Batch {batch.BatchName} thay đổi trạng thái";

            var oldStatus = evt.DomainEvent.OldStatus;
            var newStatus = evt.DomainEvent.NewStatus;
            var content = $"Trạng thái của batch {batch.BatchName} đã thay đổi từ {oldStatus} sang {newStatus}";

            List<Domain.Entities.Notification> notifications = CreateNotificationHelper.CreateForMultipleUsers(researchers, title, content, Domain.Common.Enum.NotificationTargetType.Batch, batch.ID.ToString());

            await notificationService.PushToMultipleUserAsync(researchers.Select(r => r.ID), title, content);

            notificationRepository.AddRange(notifications);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
