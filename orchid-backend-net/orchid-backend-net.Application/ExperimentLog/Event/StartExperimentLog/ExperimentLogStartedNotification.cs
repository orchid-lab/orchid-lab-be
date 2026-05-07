using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.StartExperimentLog
{
    internal class ExperimentLogStartedNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IBatchesRepository batchesRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService notificationService)
        : INotificationHandler<DomainEventNotification<ExperimentLogStarted>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogStarted> evt, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var batch = await batchesRepository.GetByIdAsync(evt.DomainEvent.BatchId, cancellationToken);
            var technician = await userRepository.GetByIdAsync(evt.DomainEvent.StartByUserId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(evt.DomainEvent.CreatedBy, cancellationToken);

            var title = "Một thí nghiệm đang được thực hiện";
            var content = $"Thí nghiệm {experimentLog.Name} của {researcher.Name} đã bắt đầu ở lồng {batch.BatchName}, được thực hiện bởi {technician.Name}";

            var researcherList = await userRepository.FindAllAsync(u => u.RoleID == 2, cancellationToken);
            var notificationList = CreateNotificationHelper.CreateForMultipleUsers(researcherList, title, content, Domain.Common.Enum.NotificationTargetType.ExperimentLog, experimentLog.ID.ToString());

            await notificationService.PushToMultipleUserAsync(
                researcherList.Select(r => r.ID), title, content);

            notificationRepository.AddRange(notificationList);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
