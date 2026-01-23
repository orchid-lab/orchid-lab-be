using MediatR;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStageChange
{
    public record SeedTaskOnStageChangeExperimentLogNotification(SeedTaskOnExperimentLogStageChanged DomainEvent) : INotification;

    internal class SeedTaskOnStageChangeExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        IMediator mediator) : INotificationHandler<SeedTaskOnStageChangeExperimentLogNotification>
    {
        public async Task Handle(SeedTaskOnStageChangeExperimentLogNotification evt, CancellationToken cancellationToken)
        {
            //get experiment log
            var experimentLog = await experimentLogRepository.FindAsync(el => el.ID == evt.DomainEvent.ExperimentLogId, cancellationToken)
                ?? throw new NotFoundException("Không thấy thí nghiệm này");
            //get method and stage order
            var method = await methodRepository.FindAsync(m => m.ID == experimentLog.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không thấy phương pháp này");

            var stage = method.MethodStages
                .SingleOrDefault(ms => ms.Order == experimentLog.CurrentStageOrder)
                ?? throw new NotFoundException("Không thấy giai đoạn này trong phương pháp");
            
            //get task template
            var tasks = await taskRepository.FindAllAsync(t => t.StageId == stage.MethodStageDefinitionId
                && t.Status == Domain.Common.Enum.TaskStatus.Template, cancellationToken);

            if (tasks is null || tasks.Count == 0)
            {
                throw new NotFoundException("Không thấy task mẫu cho giai đoạn này");
            }

            foreach(var task in tasks)
            {
                var assignment = new CreateTaskAssignmentDto
                {
                    TechnicianId = experimentLog.AssignedTo,
                    TargetType = Domain.Common.Enum.TaskTargetType.ExperimentLog,
                    TargetId = experimentLog.ID,
                    ExpectedEndDate = DateTime.UtcNow.AddDays(stage.DurationsDays),
                };

                await mediator.Send(new ConvertTaskTemplateToToDoTaskCommand(task.ID, assignment), cancellationToken);
            }
        }
    }
}
