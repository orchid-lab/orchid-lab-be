using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.Cancel
{
    internal class ExperimentLogCancelledNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService pushService)
        : INotificationHandler<DomainEventNotification<ExperimentLogCancel>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogCancel> evt, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(experiment.CreatedBy, cancellationToken);
            var technician = await userRepository.GetByIdAsync(experiment.AssignedTo, cancellationToken);

            var title = "Thí nghiệm đã bị hủy";
            var content = $"Thí nghiệm {experiment.Name} đã bị hủy bởi {technician.Name} với lý do {evt.DomainEvent.Reason}";

            var noti = CreateNotificationHelper.CreateForSingleUsers(researcher.ID, title, content, Domain.Common.Enum.NotificationTargetType.ExperimentLog, experiment.ID.ToString());
            await pushService.PushToSingleUserAsync(researcher.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
