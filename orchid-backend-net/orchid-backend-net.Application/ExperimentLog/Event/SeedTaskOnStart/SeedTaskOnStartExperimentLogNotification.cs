using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.ExperimentLog.Helper;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStart
{
    public record SeedTaskOnStartExperimentLogNotification(
        SeedTaskOnStartExperimentLogEvent DomainEvent) 
        : INotification;

    internal class SeedTaskOnStartExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        ExperimentLogSeedTask experimentLogSeedTask) 
        : INotificationHandler<SeedTaskOnStartExperimentLogNotification>
    {

        public async Task Handle(SeedTaskOnStartExperimentLogNotification evt, CancellationToken cancellationToken)
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
