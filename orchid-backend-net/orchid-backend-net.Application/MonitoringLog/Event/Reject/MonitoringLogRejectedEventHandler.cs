using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.MonitoringLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.Event.Reject
{
    /// <summary>
    /// Handles MonitoringLogRejectedEvent.
    /// Notifies technician when researcher rejects their monitoring log with reason.
    /// </summary>
    internal class MonitoringLogRejectedEventHandler(
        INotificationPushService notificationService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IMonitoringLogRepository monitoringLogRepository)
        : INotificationHandler<DomainEventNotification<MonitoringLogRejectedEvent>>
    {
        public async Task Handle(
            DomainEventNotification<MonitoringLogRejectedEvent> evt,
            CancellationToken cancellationToken)
        {
            var researcher = await userRepository.FindAsync(
                u => u.ID == evt.DomainEvent.ResearcherId, 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy researcher.");

            var monitoringLog = await monitoringLogRepository.FindAsync(
                m => m.ID == evt.DomainEvent.MonitoringLogId, 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy monitoring log.");

            var title = "Báo cáo giám sát bị từ chối";
            var content = $"Báo cáo '{monitoringLog.Name}' đã bị {researcher.Name} từ chối. " +
                         $"Lý do: {evt.DomainEvent.RejectionReason}. " +
                         "Vui lòng chỉnh sửa và gửi lại.";

            var notification = CreateNotificationHelper.CreateForSingleUsers(
                evt.DomainEvent.TechnicianId,
                title,
                content);

            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            await notificationService.PushToSingleUserAsync(
                evt.DomainEvent.TechnicianId,
                notification.Title,
                notification.Content);
        }
    }
}