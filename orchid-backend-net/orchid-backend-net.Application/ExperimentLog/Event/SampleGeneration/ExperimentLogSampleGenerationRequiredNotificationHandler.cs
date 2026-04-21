using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SampleGeneration
{
    internal class ExperimentLogSampleGenerationRequiredNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationPushService pushService,
        INotificationRepository notificationRepository
        ) : INotificationHandler<DomainEventNotification<ExperimentLogSampleGenerationRequired>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogSampleGenerationRequired> notification, CancellationToken cancellationToken)
        {
            var technician = await userRepository.GetByIdAsync(notification.DomainEvent.TechnicianId, cancellationToken);
            var experimentLog = await experimentLogRepository.GetExperimentLogByIdAsync(notification.DomainEvent.ExperimentLogId, cancellationToken);

            var title = "Thí nghiệm đã đến lúc phát sinh chồi";
            var content = $"Thí nghiệm {experimentLog.Name} đã đến lúc phát sinh chồi cho giai đoạn hiện tại với số lượng mong muốn là {notification.DomainEvent.ExpectedSampleCount}" +
                $". Vui lòng kiểm tra và thực hiện các bước tiếp theo.";

            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content);
            await pushService.PushToSingleUserAsync(technician.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
