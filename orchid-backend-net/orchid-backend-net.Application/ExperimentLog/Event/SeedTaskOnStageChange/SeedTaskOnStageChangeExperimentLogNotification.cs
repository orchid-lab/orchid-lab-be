using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.ExperimentLog.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStageChange
{
    internal class SeedTaskOnStageChangeExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        ExperimentLogSeedTask experimentLogSeedTask)
        : INotificationHandler<DomainEventNotification<SeedTaskOnExperimentLogStageChanged>>
    {
        public async Task Handle(DomainEventNotification<SeedTaskOnExperimentLogStageChanged> evt, CancellationToken cancellationToken)
        {
            //get experiment log
            var experimentLog = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);

            //get method and stage order
            var stage = await methodRepository.GetMethodStageByMethodIdAndStageOrderAsync(experimentLog.MethodId, experimentLog.CurrentStageOrder, cancellationToken);

            //find template task (where stage id == method stage definition id == template)
            var taskTemplate = await taskRepository.GetAllTaskTemplateByStageId(stage.MethodStageDefinitionId, cancellationToken);

            //seed task for experiment log assigned to
            await experimentLogSeedTask.SeedTaskAsync(taskTemplate, experimentLog, stage, cancellationToken);
        }
    }
}
