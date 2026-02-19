using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask
{
    public record ConvertTaskTemplateToToDoTaskCommand(string TaskTemplateId, CreateTaskAssignmentDto CreateTaskAssignment, string? ResearcherId = null, bool IsSeeding = false) : IRequest<string>;

    internal class ConvertTaskTemplateToToDoTaskCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IRequestHandler<ConvertTaskTemplateToToDoTaskCommand, string>
    {
        public async Task<string> Handle(ConvertTaskTemplateToToDoTaskCommand request, CancellationToken cancellationToken)
        {
            var researcherId = request.ResearcherId ?? currentUserService.UserId
                ?? throw new DomainException("Không xác định được researcher.");

            List<CreateTaskAttributeDto> createTaskAttributeList = new();

            var taskTemplate = await taskRepository.GetTemplateForConversionAsync(request.TaskTemplateId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy task template này.");
            
            TaskPolicy.ValidateTaskWorkingHour(request.CreateTaskAssignment.ExpectedEndDate, dateTimeProvider);

            Domain.Entities.Tasks taskToDo = new()
            {
                Name = taskTemplate.Name,
                Description = taskTemplate.Description,
                ResearcherId = researcherId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = researcherId,
            };

            if (taskTemplate.TaskAttributes.Count > 0)
            {

                taskTemplate.TaskAttributes.ForEach(x =>
                {
                    createTaskAttributeList.Add(
                    new CreateTaskAttributeDto()
                    {
                        ChemicalId = x.ChemicalId,
                        MaterialId = x.MaterialId,
                        Unit = x.Unit,
                        Value = x.Value,
                    });
                });
            }
            //add task assignment
            TaskAssignmentHelper.AddTaskAssignmentToTask(
                taskToDo,
                request.CreateTaskAssignment.TechnicianId,
                request.CreateTaskAssignment.TargetType,
                request.CreateTaskAssignment.TargetId,
                request.CreateTaskAssignment.ExpectedEndDate,
                DateTime.UtcNow,
                request.IsSeeding);

            //add task attribute
            TaskAttributeHelper.AddAttributesToTask(
                taskToDo,
                createTaskAttributeList);
            taskRepository.Add(taskToDo);

            //add task checklist
            TaskCheckListHelper.AddCheckListItemsToTask(
                taskToDo,
                taskTemplate.CheckList?.Items.Select(x => new CreateTaskCheckListItemDto()
                {
                    Name = x.Name,
                    Description = x.Description,
                    ExpectedMaxValue = x.ExpectedMaxValue,
                    ExpectedMinValue = x.ExpectedMinValue,
                    ExpectedUnit = x.ExpectedUnit,
                    Order = x.Order,
                }).ToList() ?? new List<CreateTaskCheckListItemDto>());

            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Tạo task thành công" : "Tạo task thất bại";
        }
    }
}
