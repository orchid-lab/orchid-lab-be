using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.CreateTask
{
    public class CreateTaskCommand(CreateTaskParameter parameter, List<CreateTaskAttributeDto> createTaskAttributes) : IRequest<string>
    {
        public required string Name { get; set; } = parameter.Name;
        public string? Description { get; set; } = parameter.Description;

        /// <summary>
        /// depends on whether the task is a to-do task or a template task
        /// </summary>
        public string? TechnicianId { get; set; } = parameter.TechnicianId;
        /// <summary>
        /// if stage id is null => the task is a to-do task
        /// if stage is not null => the task is a template task
        /// </summary>
        public string? StageId { get; set; } = parameter.StageId;
        /// <summary>
        /// if null, the task is for all experimentLog
        /// if not null, the task is for specific sample
        /// </summary>
        public string? SampleId { get; set; } = parameter.SampleId;
        public bool IsForWholeExperimentLog { get; set; } = parameter.IsForWholeExperimentLog;
        public string? ResearcherId { get; set; } = parameter.ResearcherId;
        /// <summary>
        /// Expected end date of the task - set by the creator of the task
        /// </summary>
        public DateTime ExpectedEndDate { get; set; } = parameter.ExpectedEndDate;
        public List<CreateTaskAttributeDto> TaskAttributeDtos { get; set; } = createTaskAttributes;
        public bool IsTemplateTask => !string.IsNullOrWhiteSpace(StageId);
        public bool IsToDoTask => !string.IsNullOrWhiteSpace(TechnicianId);
    }
    internal class CreateTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<CreateTaskCommand, string>
    {
        public async Task<string> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            //use case rules validation
            DateTime currentTime = TimeZoneHelper.VietnamTimeNow;
            if (!TimeZoneHelper.IsInWorkingHour(currentTime))
                throw new InvalidOperationException("Chỉ được tạo task trong giờ hành chính (7h - 17h).");

            if(request.ExpectedEndDate <= currentTime)
                throw new InvalidOperationException("Ngày dự kiến kết thúc phải sau thời điểm hiện tại.");

            if (request.IsTemplateTask && request.IsToDoTask)
                throw new InvalidOperationException("Task không thể vừa là Template vừa là To-do.");

            if (!request.IsTemplateTask && !request.IsToDoTask)
                throw new InvalidOperationException("Task phải là Template hoặc To-do.");

            var tasks = new Domain.Entities.Tasks
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                StageId = request.StageId,
                ResearcherId = request.ResearcherId,
                StartDate = currentTime,
                ExpectedEndDate = request.ExpectedEndDate,
                Status = 0
            };

            request.TaskAttributeDtos!.ForEach(attrDto =>
            {
                tasks.AddTaskAttribute(attrDto.ChemicalId, attrDto.MaterialId, attrDto.Unit, attrDto.Value);
            });

            if (request.IsToDoTask)
            {
                tasks.AddTaskAssignment(request.TechnicianId!, request.SampleId, request.IsForWholeExperimentLog);
            }

            taskRepository.Add(tasks);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Tạo task thành công" : "Tạo task thất bại";
        }
    }
}
