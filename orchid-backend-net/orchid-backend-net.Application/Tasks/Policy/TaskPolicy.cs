using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Tasks.CreateTask;
using orchid_backend_net.Application.Tasks.UpdateTask;

namespace orchid_backend_net.Application.Tasks.Policy
{
    public static class TaskPolicy
    {
        public static readonly DateTime currentTime = TimeZoneHelper.VietnamTimeNow;
        public static void ValidateTaskUpdate(Domain.Entities.Tasks task, UpdateTaskCommand request)
        {
            //giờ hành chính
            if (!TimeZoneHelper.IsInWorkingHour(currentTime))
                throw new InvalidOperationException("Chỉ được cập nhật task trong giờ hành chính (7h - 17h).");

            //expectedEndDate
            if (request.ExpectedEndDate is not null && request.ExpectedEndDate <= currentTime)
                throw new InvalidOperationException("Ngày dự kiến kết thúc phải sau thời điểm hiện tại.");

            //Template / To-do validation
            bool isChangingToDoTask = !string.IsNullOrWhiteSpace(request.SampleId);
            bool isChangingToTemplate = !string.IsNullOrWhiteSpace(request.StageId);
            bool hasAssignment = task.TaskAssignments.Count > 0;

            if (!string.IsNullOrWhiteSpace(task.StageId) && isChangingToDoTask)
                throw new InvalidOperationException("Task hiện tại đang là template task, không được chuyển thành to-do tasks.");

            if (hasAssignment && isChangingToTemplate)
                throw new InvalidOperationException("Task hiện tại đang có technician được giao, không thể chuyển thành template task.");

            //Status validation
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (!Enum.TryParse<Domain.Common.Enum.TaskStatus>(request.Status, out var status))
                {
                    throw new InvalidOperationException("Trạng thái task không hợp lệ.");
                }
                task.Status = status;
            }

            //To-do / Template conflict
            if (isChangingToDoTask && request.StageId is not null)
                throw new InvalidOperationException("Không thể chuyển to-do task thành template task.");

            if (isChangingToTemplate && request.SampleId is not null)
                throw new InvalidOperationException("Không thể chuyển thành to-do cho template task bằng cách này.");
        }

        public static void ValidateCreateTask(CreateTaskCommand request)
        {
            bool isTemplateTask = !string.IsNullOrWhiteSpace(request.StageId);
            bool isToDoTask = !string.IsNullOrWhiteSpace(request.TechnicianId);

            //use case rules validation
            if (!TimeZoneHelper.IsInWorkingHour(currentTime))
                throw new InvalidOperationException("Chỉ được tạo task trong giờ hành chính (7h - 17h).");

            if (request.ExpectedEndDate <= currentTime)
                throw new InvalidOperationException("Ngày dự kiến kết thúc phải sau thời điểm hiện tại.");

            if (isTemplateTask && isToDoTask)
                throw new InvalidOperationException("Task không thể vừa là Template vừa là To-do.");

            if (!isTemplateTask && !isToDoTask)
                throw new InvalidOperationException("Task phải là Template hoặc To-do.");
        }
    }
}
