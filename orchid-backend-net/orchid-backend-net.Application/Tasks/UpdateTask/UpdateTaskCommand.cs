using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UpdateTask
{
    /// <summary>
    /// only for update information. Not for complete task
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="createTaskAttributes"></param>
    /// <param name="updateTaskAttributes"></param>
    public class UpdateTaskCommand(
        UpdateTaskDto parameter,
        List<CreateTaskAttributeDto>? createTaskAttributes,
        List<UpdateTaskAttributeDto>? updateTaskAttributes,
        UpdateTaskAssignmentDto updateTaskAssignment) : IRequest<string>
    {
        public required string TaskId { get; set; } = parameter.TaskId;
        public string? StageId { get; set; } = parameter.StageId;
        public string? Name { get; set; } = parameter.Name;
        public string? Description { get; set; } = parameter.Description;
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; } = createTaskAttributes;
        public List<UpdateTaskAttributeDto>? UpdateTaskAttribute { get; set; } = updateTaskAttributes;
        public UpdateTaskAssignmentDto? UpdateTaskAssignment { get; set; } = updateTaskAssignment;
    }

    internal class UpdateTaskCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IRequestHandler<UpdateTaskCommand, string>
    {
        public async Task<string> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var tasks = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken) ?? throw new NotFoundException("Không tìm thấy task.");

            TaskPolicy.ValidateTaskUpdate(tasks, request, dateTimeProvider);

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
                TaskAssignmentHelper.UpdateTaskAssignmentOfTask(
                    tasks,
                    request.UpdateTaskAssignment.TaskAssignmentId!,
                    request.UpdateTaskAssignment.SampleId,
                    request.UpdateTaskAssignment.IsForWholeExperimentLog,
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
