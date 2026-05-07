using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.StagedChange
{
    internal class ExperimentLogStageChangeNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService pushService)
        : INotificationHandler<DomainEventNotification<ExperimentLogStageChanged>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogStageChanged> evt, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var technician = await userRepository.GetByIdAsync(evt.DomainEvent.TechnicianId, cancellationToken);

            var stageOrder = evt.DomainEvent.CurrentStageOrder;

            var title = "Thí nghiệm đã chuyển giai đoạn";
            var content = $"Thí nghiệm {experiment.Name} đã chuyển sang giai đoạn {stageOrder}, vui lòng kiểm tra các công việc liên quan";
            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content, Domain.Common.Enum.NotificationTargetType.ExperimentLog, experiment.ID.ToString());
            await pushService.PushToSingleUserAsync(technician.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
