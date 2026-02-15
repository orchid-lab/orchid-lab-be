using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.CreateTask
{
    public class CreateTaskCommand(
        CreateTaskDto parameter, 
        List<CreateTaskAttributeDto>? createTaskAttributes,
        CreateTaskAssignmentDto createTaskAssignment,
        List<CreateTaskCheckListItemDto> createTaskCheckListItems) : IRequest<string>
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
        public int? StageId { get; set; } = parameter.StageId;
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; } = createTaskAttributes;
        public List<CreateTaskCheckListItemDto> CreateTaskCheckListItemDtos { get; set; } = createTaskCheckListItems;
    }

    internal class CreateTaskCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IStageDefinitionRepository stageDefinitionRepository) : IRequestHandler<CreateTaskCommand, string>
    {
        public async Task<string> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            //use case rules validation
            await TaskPolicy.ValidateTaskCreate(request, dateTimeProvider, stageDefinitionRepository);

            var tasks = new Domain.Entities.Tasks
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                StageId = request.StageId,
                ResearcherId = currentUserService.UserId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId,
            };

            TaskAttributeHelper.AddAttributesToTask(tasks, request.CreateTaskAttribute);
            
            if (request.CreateTaskAssignment is not null)
            {
                TaskAssignmentHelper.AddTaskAssignmentToTask(
                    tasks,
                    request.CreateTaskAssignment.TechnicianId,
                    request.CreateTaskAssignment.TargetType,
                    request.CreateTaskAssignment.TargetId,
                    request.CreateTaskAssignment.ExpectedEndDate,
                    DateTime.UtcNow);
            }

            TaskCheckListHelper.AddCheckListItemsToTask(tasks, request.CreateTaskCheckListItemDtos);
            taskRepository.Add(tasks);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Tạo task thành công" : "Tạo task thất bại";
        }
    }
}
