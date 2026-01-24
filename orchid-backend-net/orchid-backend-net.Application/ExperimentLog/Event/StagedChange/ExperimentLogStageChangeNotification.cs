using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.StagedChange
{
    public record ExperimentLogStageChangeNotification(ExperimentLogStageChanged DomainEvent) : INotification;
    internal class ExperimentLogStageChangeNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService pushService) : INotificationHandler<ExperimentLogStageChangeNotification>
    {
        public async Task Handle(ExperimentLogStageChangeNotification evt, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(evt.DomainEvent.ResearcherId, cancellationToken);
            var technician = await userRepository.GetByIdAsync(evt.DomainEvent.TechnicianId, cancellationToken);

            var stageOrder = evt.DomainEvent.CurrentStageOrder;

            var title = "Thí nghiệm đã chuyển giai đoạn";
            var content = $"Thí nghiệm {experiment.Name} đã chuyển sang giai đoạn {stageOrder}, vui lòng kiểm tra các công việc liên quan";
            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content);
            await pushService.PushToSingleUserAsync(technician.ID,title,content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
