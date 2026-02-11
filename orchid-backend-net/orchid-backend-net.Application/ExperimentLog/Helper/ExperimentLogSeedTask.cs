using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Helper
{
    internal sealed class ExperimentLogSeedTask
    {
        private const int MaxConcurrentSeedTasks = 5;
        private readonly IServiceScopeFactory _serviceScope;
        private readonly ILogger<ExperimentLogSeedTask> _logger;

        public ExperimentLogSeedTask(
        IServiceScopeFactory scopeFactory,
        ILogger<ExperimentLogSeedTask> logger)
        {
            _serviceScope = scopeFactory;   
            _logger = logger;
        }

        public async Task SeedTaskAsync(IReadOnlyCollection<Domain.Entities.Tasks> taskTemplates, ExperimentLogs experimentLog, MethodStages stage, CancellationToken cancellationToken)
        {
            if (taskTemplates is null || taskTemplates.Count == 0)
                return;

            using var semaphore = new SemaphoreSlim(MaxConcurrentSeedTasks);

            var jobs = taskTemplates.Select(template => 
            SeedOneAsync(template, experimentLog, semaphore, stage, cancellationToken));

            await Task.WhenAll(jobs);
        }

        private async Task SeedOneAsync(
            Domain.Entities.Tasks template, 
            ExperimentLogs experimentLog,
            SemaphoreSlim semaphore, 
            MethodStages stage,
            CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                using var scope = _serviceScope.CreateScope();
                var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var assignment = BuildTaskAssignment(experimentLog, stage);
                await scopedMediator.Send(
                    new ConvertTaskTemplateToToDoTaskCommand(
                        template.ID, 
                        assignment,
                        experimentLog.CreatedBy),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError(ex, "Failed to seed task from template {TemplateID} for experiment log {ExperimentLogID}", template.ID, experimentLog.ID);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static CreateTaskAssignmentDto BuildTaskAssignment(ExperimentLogs experimentLog, MethodStages stage)
            => new()
            {
                TechnicianId = experimentLog.AssignedTo,
                TargetType = Domain.Common.Enum.TaskTargetType.ExperimentLog,
                TargetId = experimentLog.ID,
                ExpectedEndDate = DateTime.UtcNow.AddDays(stage.DurationsDays),
            };
    }
}
