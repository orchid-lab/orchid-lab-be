using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.MonitoringLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.Event.Appvored
{
    /// <summary>
    /// Handles MonitoringLogApprovedEvent.
    /// Notifies technician when researcher approves their monitoring log.
    /// </summary>
    internal class MonitoringLogApprovedEventHandler(
        INotificationPushService notificationService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IMonitoringLogRepository monitoringLogRepository)
        : INotificationHandler<DomainEventNotification<MonitoringLogApprovedEvent>>
    {
        public async Task Handle(
            DomainEventNotification<MonitoringLogApprovedEvent> evt,
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

            var title = "Báo cáo giám sát đã được duyệt";
            var content = $"Báo cáo '{monitoringLog.Name}' của bạn đã được {researcher.Name} phê duyệt.";

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