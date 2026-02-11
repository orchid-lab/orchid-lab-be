using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask
{
    public record ConvertTaskTemplateToToDoTaskCommand(string TaskTemplateId, CreateTaskAssignmentDto CreateTaskAssignment, string? ResearcherId = null) : IRequest<string>;

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
                ResearcherId = currentUserService.UserId,
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
                DateTime.UtcNow);

            //add task attribute
            TaskAttributeHelper.AddAttributesToTask(
                taskToDo,
                createTaskAttributeList);
            taskRepository.Add(taskToDo);

            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Tạo task thành công" : "Tạo task thất bại";
        }
    }
}
