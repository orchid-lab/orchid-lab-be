using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.Destroy
{
    public record ExperimentLogDestroyNotification(ExperimentLogDestroyed DomainEvent)
        : INotification;

    internal class ExperimentLogStageChangeNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService pushService) : INotificationHandler<ExperimentLogDestroyNotification>
    {
        public async Task Handle(ExperimentLogDestroyNotification evt, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(experiment.CreatedBy, cancellationToken);
            var technician = await userRepository.GetByIdAsync(experiment.AssignedTo, cancellationToken);

            var title = "Thí nghiệm đã bị hủy";
            var content = !string.IsNullOrWhiteSpace(evt.DomainEvent.Reason) 
                ? $"Thí nghiệm {experiment.Name} đã được đánh dấu hoàn thành bởi {researcher.Name} với lý do {evt.DomainEvent.Reason}"
                : $"Thí nghiệm {experiment.Name} đã được đánh dấu hoàn thành bởi {researcher.Name}";

            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content);
            await pushService.PushToSingleUserAsync(technician.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
