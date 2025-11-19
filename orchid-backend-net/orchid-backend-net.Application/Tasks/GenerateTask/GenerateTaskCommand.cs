using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Linkeds.CreateLinkedsCommand;
using orchid_backend_net.Application.TaskAttribute.CreateTaskAttribute;
using orchid_backend_net.Application.Tasks.CreateTaskWhenCreateExperimentLog;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.GenerateTask
{
    public class GenerateTaskCommand : IRequest<Unit>, ICommand
    {
        public required string ExperimentLogId { get; set; }
        public required string StageId { get; set; }
        public required List<string> TechnicianID { get; set; }
    }

    internal class GenerateTaskCommandHandler(ISender sender, ITaskTemplatesRepository taskTemplatesRepository, ITaskTemplateDetailsRepository taskTemplateDetailsRepository, IStageRepository stageRepository) : IRequestHandler<GenerateTaskCommand, Unit>
    {
        public async Task<Unit> Handle(GenerateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //get task template by stage
                var taskTemplates = await taskTemplatesRepository.FindAllAsync(template => template.StageID.Equals(request.StageId), cancellationToken);
                var taskTemplateIds = taskTemplates.Select(t => t.ID).ToHashSet();

                var taskTemplateDetails = await taskTemplateDetailsRepository.FindAllAsync(detail => taskTemplateIds.Contains(detail.TaskTemplateID), cancellationToken);
                var detailsGrouped = taskTemplateDetails
                    .GroupBy(d => d.TaskTemplateID)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var taskTemplate in taskTemplates)
                {
                    var attributes = detailsGrouped.GetValueOrDefault(taskTemplate.ID, [])
                        .Select(x => new CreateTaskAttributeCommand(name: x.Name,
                            measurementUnit: x.Unit,
                            value: (double)x.ExpectedValue,
                            description: x.Description)
                        ).ToList();

                    var taskCommand = new CreateTaskWhenCreateExperimentLogCommand(
                        name: taskTemplate.Name,
                        description: taskTemplate.Description,
                        start_date: DateTime.UtcNow,
                        end_date: DateTime.UtcNow.AddDays(1),
                        attribute: attributes,
                        technicianID: request.TechnicianID,
                        experimentLogID: request.ExperimentLogId);
                    var taskId = await sender.Send(taskCommand, cancellationToken);
                    await sender.Send(new CreateLinkedsCommand()
                    {
                        ExperimentLogID = request.ExperimentLogId,
                        TaskID = taskId,
                        StageID = request.StageId,
                    }, cancellationToken);
                }
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
