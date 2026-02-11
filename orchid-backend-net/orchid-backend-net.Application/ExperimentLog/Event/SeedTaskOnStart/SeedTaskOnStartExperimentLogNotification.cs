using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.ExperimentLog.Helper;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStart
{
    internal class SeedTaskOnStartExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        ExperimentLogSeedTask experimentLogSeedTask)
        : INotificationHandler<DomainEventNotification<SeedTaskOnStartExperimentLogEvent>>
    {
        public async Task Handle(DomainEventNotification<SeedTaskOnStartExperimentLogEvent> evt, CancellationToken cancellationToken)
        {
            //find experiment log 
            var experimentLog = await experimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);

            //determine stage order
            var stageOrder = experimentLog.CurrentStageOrder > 0 ? experimentLog.CurrentStageOrder : 1;

            //find all method stages for the method
            var methodStage = await methodRepository.GetMethodStageByMethodIdAndStageOrderAsync(experimentLog.MethodId, stageOrder, cancellationToken);

            //find template task (where stage id == method stage definition id == template)
            var templateTasks = await taskRepository.GetAllTaskTemplateByStageId(methodStage.MethodStageDefinitionId, cancellationToken);

            await experimentLogSeedTask.SeedTaskAsync(templateTasks, experimentLog, methodStage, cancellationToken);
        }
    }
}
