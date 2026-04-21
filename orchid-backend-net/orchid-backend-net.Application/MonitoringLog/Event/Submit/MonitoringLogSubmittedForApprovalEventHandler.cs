using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.MonitoringLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.Event.Submit
{
    /// <summary>
    /// Handles MonitoringLogSubmittedForApprovalEvent.
    /// Notifies researcher when technician submits or resubmits monitoring log.
    /// </summary>
    internal class MonitoringLogSubmittedForApprovalEventHandler(
        INotificationPushService notificationService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IMonitoringLogRepository monitoringLogRepository)
        : INotificationHandler<DomainEventNotification<MonitoringLogSubmittedForApprovalEvent>>
    {
        public async Task Handle(
            DomainEventNotification<MonitoringLogSubmittedForApprovalEvent> evt,
            CancellationToken cancellationToken)
        {
            var technician = await userRepository.FindAsync(
                u => u.ID == evt.DomainEvent.TechnicianId, 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy technician.");

            var monitoringLog = await monitoringLogRepository.FindAsync(
                m => m.ID == evt.DomainEvent.MonitoringLogId, 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy monitoring log.");

            // Different notification for resubmission vs initial submission
            var title = evt.DomainEvent.IsResubmission
                ? "Báo cáo giám sát đã được chỉnh sửa"
                : "Báo cáo giám sát cần được duyệt";

            var content = evt.DomainEvent.IsResubmission
                ? $"Báo cáo '{monitoringLog.Name}' đã được {technician.Name} chỉnh sửa và gửi lại, đang chờ bạn duyệt."
                : $"Báo cáo '{monitoringLog.Name}' được gửi bởi {technician.Name} đang chờ bạn duyệt.";

            var notification = CreateNotificationHelper.CreateForSingleUsers(
                evt.DomainEvent.ResearcherId,
                title,
                content);

            notificationRepository.Add(notification);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            await notificationService.PushToSingleUserAsync(
                evt.DomainEvent.ResearcherId,
                notification.Title,
                notification.Content);
        }
    }
}