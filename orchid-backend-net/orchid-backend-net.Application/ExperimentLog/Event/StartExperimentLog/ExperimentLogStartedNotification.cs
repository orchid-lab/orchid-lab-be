using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.StartExperimentLog
{
    public record ExperimentLogStartedNotification(ExperimentLogStarted DomainEvent)
        : INotification;
    internal class ExperimentLogStartedNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IBatchesRepository batchesRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService notificationService)
        : INotificationHandler<ExperimentLogStartedNotification>
    {
        public async Task Handle(ExperimentLogStartedNotification evt, CancellationToken cancellationToken)
        {
            //get experiment log
            var experimentLog = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);

            //get batch
            var batch = await batchesRepository.GetByIdAsync(evt.DomainEvent.BatchId, cancellationToken);

            //get technician assign to experiment log
            var technician = await userRepository.GetByIdAsync(evt.DomainEvent.StartByUserId, cancellationToken);

            //get researcher name to inform title and content
            var researcher = await userRepository.GetByIdAsync(evt.DomainEvent.CreatedBy, cancellationToken);

            var title = "Một thí nghiệm đang được thực hiện";
            var content = $"Thí nghiệm {experimentLog.Name} của {researcher.Name} đã bắt đầu ở lồng {batch.BatchName}, được thực hiện bởi {technician.Name}";

            //get all technician for notification
            var researcherList = await userRepository.FindAllAsync(u => u.RoleID == 2, cancellationToken);

            // create notification list
            var notificationList = CreateNotificationHelper.CreateForMultipleUsers(researcherList, title, content);

            // push notification to each researcher
            await notificationService.PushToMultipleUserAsync(
                researcherList.Select(r => r.ID),
                title,
                content);

            notificationRepository.AddRange(notificationList);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
