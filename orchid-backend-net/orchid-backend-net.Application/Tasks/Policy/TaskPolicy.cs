using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.UseCase.ChangeTaskStatus;
using orchid_backend_net.Application.Tasks.UseCase.CreateTask;
using orchid_backend_net.Application.Tasks.UseCase.UpdateTask;
using orchid_backend_net.Domain.IRepositories;

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
            Domain.Common.Enum.TaskStatus.ReworkRequired,
            Domain.Common.Enum.TaskStatus.DeclinedByTechnician,
        ];

        /// <summary>
        /// validate for task update, check that task is template or TO-DO
        /// </summary>
        /// <param name="task"></param>
        /// <param name="request"></param>
        /// <param name="dateTimeProvider"></param>
        /// <param name="stageDefinitionRepository"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async static Task ValidateTaskUpdate(
            Domain.Entities.Tasks task,
            UpdateTaskCommand request,
            IDateTimeProvider dateTimeProvider,
            IStageDefinitionRepository stageDefinitionRepository)
        {
            bool taskIsTemplate = task.StageId is not null;
            bool taskIsToDo = task.TaskAssignment != null;

            bool requestHasAssignmentUpdate = request.UpdateTaskAssignment is not null;
            bool requestHasStageUpdate = request.StageId is not null 
                && await stageDefinitionRepository.AnyAsync(s => request.StageId == s.ID);

            // RULE 1: Invariant
            if (taskIsTemplate == taskIsToDo)
                throw new InvalidOperationException(
                    "Task đang ở trạng thái không hợp lệ (vừa template vừa to-do hoặc không cái nào).");

            // RULE 2: Template → To-do (forbidden)
            if (taskIsTemplate && requestHasAssignmentUpdate)
                throw new InvalidOperationException(
                    "Không thể thêm task assignment vào template task.");

            // RULE 3: To-do → Template (forbidden)
            if (taskIsToDo && requestHasStageUpdate)
                throw new InvalidOperationException(
                    "Không thể gán Stage cho to-do task.");

            // RULE 4: Validate assignment data ONLY if updated
            if (requestHasAssignmentUpdate)
            {
                ValidateTaskWorkingHour(
                    request.UpdateTaskAssignment!.ExpectedEndDate,
                    dateTimeProvider);
            }
        }


        /// <summary>
        /// validate for create task 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="dateTimeProvider"></param>
        /// <param name="stageDefinitionRepository"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task ValidateTaskCreate(CreateTaskCommand request, IDateTimeProvider dateTimeProvider, IStageDefinitionRepository stageDefinitionRepository)
        {
            bool isTemplateTask = request.StageId is not null;
            bool isToDoTask = request.CreateTaskAssignment is not null;
            bool isStageExist = await stageDefinitionRepository.AnyAsync(s => request.StageId == s.ID);

            //rule 1: task must be template or to-do task
            if (isTemplateTask == isToDoTask)
                throw new InvalidOperationException(
                    "Task phải là Template hoặc To-do, không được đồng thời hoặc không cái nào.");

            //rule 2: if task template => stage must exist
            if (isStageExist != isTemplateTask)
            {
                throw new InvalidOperationException("Không tìm thấy stage id.");
            }

            // Rule 2: to-do task => must have assignment
            if (isToDoTask)
            {
                ValidateTaskWorkingHour(
                    request.CreateTaskAssignment!.ExpectedEndDate,
                    dateTimeProvider);
            }
        }

        /// <summary>
        /// validate for task change status
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="request"></param>
        /// <param name="dateTimeProvider"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Domain.Common.Enum.TaskStatus ValidateTaskStatusChange(Domain.Entities.Tasks tasks, ChangeTaskStatusCommand request, IDateTimeProvider dateTimeProvider)
        {
            ValidateTaskWorkingHour(null, dateTimeProvider);
            bool IsToDoTask = tasks.TaskAssignment != null;
            //only to-do task can change status
            if (!IsToDoTask)
                throw new InvalidOperationException("Chỉ có to-do task mới có thể thay đổi trạng thái");

            if (!Enum.TryParse<Domain.Common.Enum.TaskStatus>(request.Status, out var parsedStatus))
                throw new InvalidOperationException("Trạng thái task không hợp lệ.");

            if (!AllowedNextStatuses.Contains(parsedStatus))
                throw new InvalidOperationException("Không thể chuyển trạng thái về lại mới tạo hoặc xóa.");

            // If completing → must have EndDate
            if (IsCompletedStatus(parsedStatus) && request.EndDate == null)
                throw new InvalidOperationException("Thiếu ngày kết thúc khi hoàn thành task.");

            // Validate working hour only when completing
            if (IsCompletedStatus(parsedStatus))
            {
                ValidateTaskWorkingHour(request.EndDate, dateTimeProvider);
            }

            return parsedStatus;
        }

        /// <summary>
        /// validate working hour
        /// </summary>
        /// <param name="expectedEndDate"></param>
        /// <param name="dateTimeProvider"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ValidateTaskWorkingHour(DateTime? expectedEndDate, IDateTimeProvider dateTimeProvider)
        {
            DateTime currentTime = dateTimeProvider.Now;

            //if (!dateTimeProvider.IsInWorkingHour(currentTime))
            //    throw new InvalidOperationException("Chỉ được thao tác với task trong giờ hành chính (7h - 17h).");


            if (expectedEndDate is not null && expectedEndDate <= currentTime)
                throw new InvalidOperationException("Ngày dự kiến kết thúc phải sau thời điểm hiện tại.");
        }

        public static bool IsCompletedStatus(Domain.Common.Enum.TaskStatus status)
            => status is Domain.Common.Enum.TaskStatus.CompletedInTime or Domain.Common.Enum.TaskStatus.CompletedOutTime;
    }
}
