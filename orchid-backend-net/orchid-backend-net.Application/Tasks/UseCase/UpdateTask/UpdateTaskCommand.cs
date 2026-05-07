using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
using System.Diagnostics.CodeAnalysis;

namespace orchid_backend_net.Application.Tasks.UseCase.UpdateTask
{
    /// <summary>
    /// only for update information. Not for complete task
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="createTaskAttributes"></param>
    /// <param name="updateTaskAttributes"></param>
    public class UpdateTaskCommand : IRequest<string>
    {
        [SetsRequiredMembers]
        public UpdateTaskCommand()
        {
            TaskId = string.Empty;
        }

        [SetsRequiredMembers]
        public UpdateTaskCommand(
            UpdateTaskDto parameter,
            List<CreateTaskAttributeDto>? createTaskAttributes,
            List<UpdateTaskAttributeDto>? updateTaskAttributes,
            UpdateTaskAssignmentDto? updateTaskAssignment)
        {
            TaskId = parameter.TaskId;
            StageId = parameter.StageId;
            Name = parameter.Name;
            Description = parameter.Description;
            CreateTaskAttribute = createTaskAttributes;
            UpdateTaskAttribute = updateTaskAttributes;
            UpdateTaskAssignment = updateTaskAssignment;
        }

        public required string TaskId { get; set; }
        public int? StageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; }
        public List<UpdateTaskAttributeDto>? UpdateTaskAttribute { get; set; }
        public UpdateTaskAssignmentDto? UpdateTaskAssignment { get; set; }
    }

    internal class UpdateTaskCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IStageDefinitionRepository stageDefinitionRepository) : IRequestHandler<UpdateTaskCommand, string>
    {
        public async Task<string> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var tasks = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken) ?? throw new NotFoundException("Không tìm thấy task.");

           await TaskPolicy.ValidateTaskUpdate(tasks, request, dateTimeProvider, stageDefinitionRepository);

            //update basic info
            tasks.Name = request.Name ?? tasks.Name;
            tasks.Description = request.Description ?? tasks.Description;
            tasks.UpdatedDate = DateTime.UtcNow;
            tasks.UpdatedBy = currentUserService.UserId;
            //create and update task attributes
            TaskAttributeHelper.AddAttributesToTask(tasks, request.CreateTaskAttribute);
            TaskAttributeHelper.UpdateAttributesOfTask(tasks, request.UpdateTaskAttribute);

            //only update sample and scope; do not change assigned technician
            if (request.UpdateTaskAssignment is not null &&
                !string.IsNullOrWhiteSpace(request.UpdateTaskAssignment.TaskAssignmentId))
            {
                TaskAssignmentHelper.ReassignTaskAssignmentToTask(
                    tasks,
                    request.UpdateTaskAssignment.TargetType,
                    request.UpdateTaskAssignment.TargetId,
                    request.UpdateTaskAssignment.ExpectedEndDate,
                    null
                );
            }

            taskRepository.Update(tasks);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Cập nhật task thành công" : "Cập nhật task thất bại";
        }
    }
}
