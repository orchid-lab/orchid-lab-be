using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.Complete
{
    internal class ExperimentLogCompletedNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        INotificationPushService pushService)
        : INotificationHandler<DomainEventNotification<ExperimentLogCompleted>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogCompleted> evt, CancellationToken cancellationToken)
        {
            var experiment = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await userRepository.GetByIdAsync(experiment.CreatedBy, cancellationToken);
            var technician = await userRepository.GetByIdAsync(experiment.AssignedTo, cancellationToken);

            var title = "Thí nghiệm đã hoàn thành";
            var content = $"Thí nghiệm {experiment.Name} đã được đánh dấu hoàn thành bởi {researcher.Name}";
            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content);
            await pushService.PushToSingleUserAsync(technician.ID, title, content);
            notificationRepository.Add(noti);

            //create cleanning task for technician 
            var cleaningTask = new Domain.Entities.Tasks
            {
                Name = $"Dọn dẹp sau thí nghiệm {experiment.Name}",
                Description = $"Dọn dẹp và chuẩn bị lại khu vực sau khi hoàn thành thí nghiệm {experiment.Name}",
                CreatedBy = researcher.ID,
            };
            cleaningTask.AddTaskAssignment(
                evt.DomainEvent.TechnicianId, 
                Domain.Common.Enum.TaskTargetType.ExperimentLog, 
                evt.DomainEvent.ExperimentLogId, 
                DateTime.UtcNow.AddDays(3), 
                DateTime.UtcNow, 
                true);
            taskRepository.Add(cleaningTask);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
