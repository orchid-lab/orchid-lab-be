using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.CreateTask;
using orchid_backend_net.Application.Tasks.UpdateTask;

namespace orchid_backend_net.Application.Tasks.Policy
{
    public static class TaskPolicy
    {
        private static readonly HashSet<Domain.Common.Enum.TaskStatus> AllowedNextStatuses =
        [
            Domain.Common.Enum.TaskStatus.InProgress,
            Domain.Common.Enum.TaskStatus.WaitingForApproval,
            Domain.Common.Enum.TaskStatus.CompletedInTime,
            Domain.Common.Enum.TaskStatus.CompletedOutTime,
        ];

        public static void ValidateTaskUpdate(Domain.Entities.Tasks task, UpdateTaskCommand request, IDateTimeProvider dateTimeProvider)
        {
            var assignment = request.UpdateTaskAssignment;

            bool taskIsTemplate = !string.IsNullOrWhiteSpace(task.StageId);
            bool taskHasAssignment = task.TaskAssignments.Count != 0;

            bool wantToBeToDo =
                assignment is not null &&
                !string.IsNullOrWhiteSpace(assignment.SampleId);

            bool wantToBeTemplate =
                !string.IsNullOrWhiteSpace(request.StageId);

            // Template → To-do (KHÔNG cho phép)
            if (taskIsTemplate && wantToBeToDo)
                throw new InvalidOperationException(
                    "Task hiện tại đang là template task, không được chuyển thành to-do task.");

            // Đã có technician → KHÔNG cho về template
            if (taskHasAssignment && wantToBeTemplate)
                throw new InvalidOperationException(
                    "Task hiện tại đang có technician được giao, không thể chuyển thành template task.");

            // Working hour + expected date
            ValidateTaskWorkingHour(assignment?.ExpectedEndDate, dateTimeProvider);
        }

        public static void ValidateTaskCreate(CreateTaskCommand request, IDateTimeProvider dateTimeProvider)
        {
            bool isTemplateTask = !string.IsNullOrWhiteSpace(request.StageId);
            bool isToDoTask = !string.IsNullOrWhiteSpace(request.CreateTaskAssignment.TechnicianId);

            //use case rules validation
            ValidateTaskWorkingHour(request.CreateTaskAssignment.ExpectedEndDate, dateTimeProvider);

            if (isTemplateTask && isToDoTask)
                throw new InvalidOperationException("Task không thể vừa là Template vừa là To-do.");

            if (!isTemplateTask && !isToDoTask)
                throw new InvalidOperationException("Task phải là Template hoặc To-do.");
        }

        public static Domain.Common.Enum.TaskStatus ValidateTaskStatusChange(string status, IDateTimeProvider dateTimeProvider)
        {
            ValidateTaskWorkingHour(null, dateTimeProvider);

            if (!Enum.TryParse<Domain.Common.Enum.TaskStatus>(status, out var parsedStatus))
                throw new InvalidOperationException("Trạng thái task không hợp lệ.");

            if (!AllowedNextStatuses.Contains(parsedStatus))
                throw new InvalidOperationException("Không thể chuyển trạng thái về lại mới tạo hoặc xóa.");

            return parsedStatus;
        }

        public static void ValidateTaskWorkingHour(DateTime? expectedEndDate, IDateTimeProvider dateTimeProvider)
        {
            DateTime currentTime = dateTimeProvider.Now;

            if (!dateTimeProvider.IsInWorkingHour(currentTime))
                throw new InvalidOperationException("Chỉ được thao tác với task trong giờ hành chính (7h - 17h).");

            if (expectedEndDate is not null && expectedEndDate <= currentTime)
                throw new InvalidOperationException("Ngày dự kiến kết thúc phải sau thời điểm hiện tại.");
        }
    }
}
