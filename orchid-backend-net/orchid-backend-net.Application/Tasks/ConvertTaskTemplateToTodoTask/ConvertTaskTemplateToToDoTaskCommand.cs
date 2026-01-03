using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.ConvertTaskTemplateToTodoTask
{
    public class ConvertTaskTemplateToToDoTaskCommand(string taskTemplateId, CreateTaskAssignmentDto createTaskAssignment) : IRequest<string>
    {
        public required string TaskTemplateId { get; set; } = taskTemplateId;
        public CreateTaskAssignmentDto CreateTaskAssignment { get; set; } = createTaskAssignment;
    }

    internal class ConvertTaskTemplateToToDoTaskCommandHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService) : IRequestHandler<ConvertTaskTemplateToToDoTaskCommand, string>
    {
        public async Task<string> Handle(ConvertTaskTemplateToToDoTaskCommand request, CancellationToken cancellationToken)
        {
            List<CreateTaskAttributeDto> createTaskAttributeList = [];

            var taskTemplate = await taskRepository.GetTemplateForConversionAsync(request.TaskTemplateId, cancellationToken);
            if (taskTemplate is null)
                throw new NotFoundException("Không tìm thấy task template này.");

            TaskPolicy.ValidateTaskWorkingHour(request.CreateTaskAssignment.ExpectedEndDate);

            Domain.Entities.Tasks taskToDo = new()
            {
                Name = taskTemplate.Name,
                Description = taskTemplate.Description,
                ResearcherId = currentUserService.UserId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId
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
                request.CreateTaskAssignment.SampleId,
                request.CreateTaskAssignment.IsForWholeExperimentLog,
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
