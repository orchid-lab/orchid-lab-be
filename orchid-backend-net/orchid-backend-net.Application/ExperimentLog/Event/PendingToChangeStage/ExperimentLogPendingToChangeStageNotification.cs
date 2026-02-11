using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.PendingToChangeStage
{
    internal class ExperimentLogPendingToChangeStageNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationPushService pushService,
        INotificationRepository notificationRepository)
        : INotificationHandler<DomainEventNotification<ExperimentLogPendingToChangeStage>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogPendingToChangeStage> evt, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(evt.DomainEvent.CreatedBy, cancellationToken);
            var technician = await userRepository.GetByIdAsync(evt.DomainEvent.AssignTo, cancellationToken);

            var title = "Thí nghiệm cần chuyển giai đoạn";
            var content = $"Thí nghiệm {experimentLog.Name} đã được yêu cầu chuyển sang giai đoạn tiếp theo bởi {technician.Name}";

            var noti = CreateNotificationHelper.CreateForSingleUsers(researcher.ID, title, content);
            await pushService.PushToSingleUserAsync(researcher.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
