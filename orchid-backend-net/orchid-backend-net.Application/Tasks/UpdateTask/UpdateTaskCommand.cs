using MediatR;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UpdateTask
{
    public class UpdateTaskCommand(UpdateTaskDto parameter, List<CreateTaskAttributeDto>? createTaskAttributes,  List<UpdateTaskAttributeDto>? updateTaskAttributes) : IRequest<string>
    {
        public required string TaskId { get; set; } = parameter.TaskId;
        public string? TaskAssignmentId { get; set; } = parameter.TaskAssignmentId;
        public string? StageId { get; set; } = parameter.StageId;
        public string? SampleId { get; set; } = parameter.SampleId;
        public string? Name { get; set; } = parameter.Name;
        public string? Description { get; set; } = parameter.Description;
        public string? Status { get; set; } = parameter.Status;
        public DateTime? ExpectedEndDate { get; set; } = parameter.ExpectedEndDate;
        public bool IsForWholeExperimentLog { get; set; } = parameter.IsForWholeExperimentLog;
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; } = createTaskAttributes;
        public List<UpdateTaskAttributeDto>? UpdateTaskAttribute { get; set; } = updateTaskAttributes;
    }

    internal class UpdateTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<UpdateTaskCommand, string>
    {
        public async Task<string> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var tasks = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken) ?? throw new NotFoundException("Không tìm thấy task.");

            TaskPolicy.ValidateTaskUpdate(tasks, request);

            //status validation
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (!Enum.TryParse<Domain.Common.Enum.TaskStatus>(request.Status, out var status))
                {
                    throw new InvalidOperationException("Trạng thái task không hợp lệ.");
                }
                tasks.Status = status;
            }

            //update basic info
            tasks.Name = request.Name ?? tasks.Name;
            tasks.Description = request.Description ?? tasks.Description;
            tasks.ExpectedEndDate = request.ExpectedEndDate ?? tasks.ExpectedEndDate;

            //create and update task attributes
            TaskAttributeHelper.AddAttributesToTask(tasks, request.CreateTaskAttribute);
            TaskAttributeHelper.UpdateAttributesOfTask(tasks, request.UpdateTaskAttribute);

            //only update sample and scope; do not change assigned technician
            TaskAssignmentHelper.UpdateTaskAssignmentOfTask(tasks, request.TaskAssignmentId, request.SampleId, request.IsForWholeExperimentLog);

            taskRepository.Update(tasks);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Cập nhật task thành công" : "Cập nhật task thất bại";
        }
    }
}
