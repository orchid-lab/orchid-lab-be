using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.Complete
{
    public record ExperimentLogCompletedNotification(ExperimentLogCompleted DomainEvent)
        : INotification;

    internal class ExperimentLogCompletedNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService pushService)
        : INotificationHandler<ExperimentLogCompletedNotification>
    {
        public async Task Handle(ExperimentLogCompletedNotification evt, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(experiment.CreatedBy, cancellationToken);
            var technician = await userRepository.GetByIdAsync(experiment.AssignedTo, cancellationToken);

            var title = "Thí nghiệm đã hoàn thành";
            var content = $"Thí nghiệm {experiment.Name} đã được đánh dấu hoàn thành bởi {researcher.Name}";
            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content);
            await pushService.PushToSingleUserAsync(technician.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
