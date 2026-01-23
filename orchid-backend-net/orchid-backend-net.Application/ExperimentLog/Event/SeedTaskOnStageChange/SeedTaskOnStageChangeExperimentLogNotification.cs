using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStageChange
{
    public record SeedTaskOnStageChangeExperimentLogNotification(SeedTaskOnExperimentLogStageChanged DomainEvent) : INotification;

    internal class SeedTaskOnStageChangeExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        IServiceScopeFactory scopeFactory,
        ILogger<SeedTaskOnStageChangeExperimentLogNotificationHandler> logger) : INotificationHandler<SeedTaskOnStageChangeExperimentLogNotification>
    {
        private const int MaxConcurrentSeedTasks = 5;

        public async Task Handle(SeedTaskOnStageChangeExperimentLogNotification evt, CancellationToken cancellationToken)
        {
            //get experiment log
            var experimentLog = await GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);

            //get method and stage order
            var stage = await GetMethodStageAsync(experimentLog.MethodId, experimentLog.CurrentStageOrder, cancellationToken);

            //get task template
            var tasks = await GetTaskTemplatesByStageIdAsync(stage.ID, cancellationToken);

            if (tasks is null || tasks.Count == 0)
            {
                throw new NotFoundException("Không thấy task mẫu cho giai đoạn này");
            }

            //seed task for experiment log assigned to
            using var semaphore = new SemaphoreSlim(MaxConcurrentSeedTasks); // limit parrallel thread in pool
            var sendTasks = tasks.Select(async template =>
            {
                await semaphore.WaitAsync();
                try
                {
                    // Use a fresh scope per task to avoid sharing scoped services (e.g. DbContext)
                    using var scope = scopeFactory.CreateScope();
                    var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    var assignment = BuildTaskAssignment(experimentLog, stage);
                    await scopedMediator.Send(new ConvertTaskTemplateToToDoTaskCommand(template.ID, assignment), cancellationToken);
                }
                catch(Exception ex)
                {
                    // Log error
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
