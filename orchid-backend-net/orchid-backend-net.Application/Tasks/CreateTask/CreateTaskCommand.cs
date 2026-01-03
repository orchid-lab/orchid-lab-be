using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.CreateTask
{
    public class CreateTaskCommand(CreateTaskDto parameter, List<CreateTaskAttributeDto>? createTaskAttributes, CreateTaskAssignmentDto createTaskAssignment) : IRequest<string>
    {
        public string Name { get; set; } = parameter.Name;
        public string? Description { get; set; } = parameter.Description;

        /// <summary>
        /// depends on whether the task is a to-do task or a template task
        /// </summary>
        public CreateTaskAssignmentDto? CreateTaskAssignment { get; set; } = createTaskAssignment;
        /// <summary>
        /// if stage id is null => the task is a to-do task
        /// if stage is not null => the task is a template task
        /// </summary>
        public string? StageId { get; set; } = parameter.StageId;
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; } = createTaskAttributes;
    }

    internal class CreateTaskCommandHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService) : IRequestHandler<CreateTaskCommand, string>
    {
        public async Task<string> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            //use case rules validation
            TaskPolicy.ValidateTaskCreate(request);
            var tasks = new Domain.Entities.Tasks
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                StageId = request.StageId,
                ResearcherId = currentUserService.UserId,
                Status = Domain.Common.Enum.TaskStatus.Created,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId,
            };

            TaskAttributeHelper.AddAttributesToTask(tasks, request.CreateTaskAttribute);
            TaskAssignmentHelper.AddTaskAssignmentToTask(
                tasks, 
                request.CreateTaskAssignment.TechnicianId, 
                request.CreateTaskAssignment.SampleId,
                request.CreateTaskAssignment.IsForWholeExperimentLog, 
                request.CreateTaskAssignment.ExpectedEndDate,
                DateTime.UtcNow);
            taskRepository.Add(tasks);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Tạo task thành công" : "Tạo task thất bại";
        }
    }
}
