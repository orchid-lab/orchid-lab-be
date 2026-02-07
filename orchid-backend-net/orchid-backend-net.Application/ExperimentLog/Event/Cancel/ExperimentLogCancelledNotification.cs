using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.ExperimentLog.Event.Cancel
{
    public record ExperimentLogCancelledNotification(ExperimentLogCancel DomainEvent) 
        : INotification;
    internal class ExperimentLogCancelledNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        INotificationPushService pushService) : INotificationHandler<ExperimentLogCancelledNotification>
    {
        public async Task Handle(ExperimentLogCancelledNotification notification, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(notification.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(experiment.CreatedBy, cancellationToken);
            var technician = await userRepository.GetByIdAsync(experiment.AssignedTo, cancellationToken);

            var title = "Thí nghiệm đã bị hủy";
            var content = $"Thí nghiệm {experiment.Name} đã bị hủy bởi {technician.Name} với lý do {notification.DomainEvent.Reason}";

            var noti = CreateNotificationHelper.CreateForSingleUsers(researcher.ID, title, content);
            await pushService.PushToSingleUserAsync(researcher.ID, title, content);
            notificationRepository.Add(noti);
            await notificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
