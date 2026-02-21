using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Policy
{
    public static class ExperimentLogPolicy
    {
        private static readonly HashSet<ExperimentLogStatus> ValidStatusTransitions = [
            ExperimentLogStatus.Created,
            ExperimentLogStatus.InProgress,
            ExperimentLogStatus.WaitingForChangeStage,
            ExperimentLogStatus.ConfirmChangeStage,
            ExperimentLogStatus.Completed,
            ExperimentLogStatus.Destroyed
            ];

        private static readonly HashSet<Domain.Common.Enum.TaskStatus> CompletedTaskStatus = [
            Domain.Common.Enum.TaskStatus.Deleted,
            Domain.Common.Enum.TaskStatus.DeclinedByTechnician,
            Domain.Common.Enum.TaskStatus.CompletedInTime,
            Domain.Common.Enum.TaskStatus.CompletedOutTime
            ];

        /// <summary>
        /// this is using for validate the status giving in API, to make sure that the status is valid and can be changed to.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ExperimentLogStatus ValidateStatusChange(string status)
        {
            if (!Enum.TryParse<ExperimentLogStatus>(status, out var newStatus) ||
                !ValidStatusTransitions.Contains(newStatus))
            {
                throw new ArgumentException("Trạng thái thí nghiệm không hợp lệ.");
            }
            return newStatus;
        }

        /// <summary>
        /// this method is using for validate the status of experiment log in WaitingForChangeStage, 
        /// to make sure that there is no incomplete task that is related to the experiment log, 
        /// if there is any incomplete task, it will throw an exception and prevent the stage from changing.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskRepository"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="DomainException"></exception>
        public static async Task ValidateForChangeStage(string id, ITaskRepository taskRepository, CancellationToken cancellationToken)
        {
            var hasIncompleteTask = await taskRepository.AnyAsync(
                queryOptions: q =>
                    q.Where(t =>
                        t.TaskAssignment.TargetType == TaskTargetType.ExperimentLog
                        && t.TaskAssignment.TargetId.Equals(id)
                        && !CompletedTaskStatus.Contains(t.Status)
                        && t.Status != Domain.Common.Enum.TaskStatus.Template 
                    ),
                cancellationToken);

            if (hasIncompleteTask)
            {
                throw new DomainException(
                    "Không thể chuyển giai đoạn khi còn có tasks đang thực hiện hoặc chờ phê duyệt. " +
                    "Vui lòng hoàn thành tất cả tasks trước khi chuyển stage."
                );
            }
        }
    }
}
