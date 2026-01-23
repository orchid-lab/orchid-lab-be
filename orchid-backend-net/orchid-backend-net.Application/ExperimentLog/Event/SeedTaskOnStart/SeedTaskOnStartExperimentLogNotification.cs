using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStart
{
    public record SeedTaskOnStartExperimentLogNotification(SeedTaskOnStartExperimentLogEvent DomainEvent) : INotification;

    internal class SeedTaskOnStartExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        IServiceScopeFactory scopeFactory,
        ILogger<SeedTaskOnStartExperimentLogNotificationHandler> logger) : INotificationHandler<SeedTaskOnStartExperimentLogNotification>
    {
        private const int MaxConcurrentSeedTasks = 5;

        public async Task Handle(SeedTaskOnStartExperimentLogNotification evt, CancellationToken cancellationToken)
        {
            //find experiment log 
            var experimentLog = await GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);

            //determine stage order
            var stageOrder = experimentLog.CurrentStageOrder > 0 ? experimentLog.CurrentStageOrder : 1;

            //find all method stages for the method
            var methodStage = await GetMethodStageAsync(experimentLog.MethodId, stageOrder, cancellationToken);

            //find template task (where stage id == method stage definition id == template)
            var templateTasks = await GetTaskTemplatesByStageIdAsync(methodStage.MethodStageDefinitionId, cancellationToken);

            if (templateTasks is null || templateTasks.Count == 0)
                return;

            // seed task for experiment log assigned to
            var semaphore = new SemaphoreSlim(MaxConcurrentSeedTasks); // Giới hạn số lượng tác vụ đồng thời
            var sendTasks = templateTasks.Select(async template =>
            {
                await semaphore.WaitAsync();
                try
                {
                    // use a fresh scope per task to avoid sharing scoped services (e.g. DbContext)
                    using var scope = scopeFactory.CreateScope();
                    var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var assignment = BuildTaskAssignment(experimentLog, methodStage);
                    await scopedMediator.Send(new ConvertTaskTemplateToToDoTaskCommand(template.ID, assignment), cancellationToken);
                }
                catch (Exception ex)
                {
                    // log error
                    logger.LogError(ex, $"Failed to seed task from template {template.ID} for ExperimentLog {experimentLog.ID}");
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();

            await Task.WhenAll(sendTasks);
        }

        private static CreateTaskAssignmentDto BuildTaskAssignment(ExperimentLogs experimentLog, MethodStages stage)
            => new()
            {
                TechnicianId = experimentLog.AssignedTo,
                TargetType = Domain.Common.Enum.TaskTargetType.ExperimentLog,
                TargetId = experimentLog.ID,
                ExpectedEndDate = DateTime.UtcNow.AddDays(stage.DurationsDays),
            };

        private async Task<ExperimentLogs> GetExperimentLogByIdAsync(string id, CancellationToken cancellationToken)
            => await experimentLogRepository.FindAsync(el => el.ID == id, cancellationToken)
                ?? throw new NotFoundException("Không thấy thí nghiệm này");

        private async Task<MethodStages> GetMethodStageAsync(int methodId, int experimentLogCurrentStageOrder, CancellationToken cancellationToken)
        {
            var method = await methodRepository.FindAsync(m => m.ID == methodId, cancellationToken)
                ?? throw new NotFoundException("Không thấy phương pháp này");

            var stage = method.MethodStages
                .SingleOrDefault(ms => ms.Order == experimentLogCurrentStageOrder)
                ?? throw new NotFoundException("Không thấy giai đoạn này trong phương pháp");

            return stage;
        }

        private async Task<List<Domain.Entities.Tasks>> GetTaskTemplatesByStageIdAsync(int stageId, CancellationToken cancellationToken)
            => await taskRepository.FindAllAsync(t => t.StageId == stageId
                && t.Status == Domain.Common.Enum.TaskStatus.Template, cancellationToken);
    }
}
