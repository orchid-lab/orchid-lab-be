using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.SeedTaskOnStart
{
    public record SeedTaskOnStartExperimentLogNotification(SeedTaskOnStartExperimentLogEvent DomainEvent) : INotification;

    internal class SeedTaskOnStartExperimentLogNotificationHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        ITaskRepository taskRepository,
        IMediator mediator) : INotificationHandler<SeedTaskOnStartExperimentLogNotification>
    {
        public async Task Handle(SeedTaskOnStartExperimentLogNotification evt, CancellationToken cancellationToken)
        {
            //find experiment log 
            var experimentLog = await experimentLogRepository.FindAsync(el => el.ID == evt.DomainEvent.ExperimentLogId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy thí nghiệm này");

            //determine stage order
            var stageOrder = experimentLog.CurrentStageOrder > 0 ? experimentLog.CurrentStageOrder : 1;
            
            //find all method stages for the method
            var method = await methodRepository.FindAsync(m => m.ID == experimentLog.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy phương pháp này");

            var methodStages = method.MethodStages.SingleOrDefault(ms => ms.Order == stageOrder)
                ?? throw new NotFoundException("Không tìm thấy giai đoạn phương pháp này");

            //find template task (where stage id == method stage definition id == template)
            var templateTask = await taskRepository.FindAllAsync(
                t => t.StageId == methodStages.MethodStageDefinitionId 
                && t.Status == Domain.Common.Enum.TaskStatus.Template, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy công việc mẫu cho giai đoạn này");

            if (templateTask is null || templateTask.Count == 0)
                return;
            //notify to create task from template
            //also seed task
            foreach(var template in templateTask)
            {
                var assignment = new CreateTaskAssignmentDto
                {
                    TechnicianId = experimentLog.AssignedTo,
                    TargetType = Domain.Common.Enum.TaskTargetType.ExperimentLog,
                    TargetId = experimentLog.ID,
                    ExpectedEndDate = DateTime.UtcNow.AddDays(methodStages.DurationsDays)
                };

                await mediator.Send(new ConvertTaskTemplateToToDoTaskCommand(template.ID, assignment), cancellationToken);
            }
        }
    }
}
