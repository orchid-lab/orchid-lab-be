namespace orchid_backend_net.Application.Tasks.Helper
{
    public static class TaskStatusActionDispatcher
    {
        public static void Dispatch(
            Domain.Entities.Tasks task,
            Domain.Common.Enum.TaskStatus nextStatus,
            string currentUserId,
            DateTime? endDate)
        {
            switch (nextStatus)
            {
                case Domain.Common.Enum.TaskStatus.InProgress:
                    task.AcceptTask(currentUserId);
                    break;

                case Domain.Common.Enum.TaskStatus.DeclinedByTechnician:
                    task.DeclineTask(currentUserId, "Technician declined task");
                    break;

                case Domain.Common.Enum.TaskStatus.WaitingForApproval:
                    task.ReportTask(currentUserId);
                    break;

                case Domain.Common.Enum.TaskStatus.CompletedInTime:
                case Domain.Common.Enum.TaskStatus.CompletedOutTime:
                    task.Complete(currentUserId, endDate ?? DateTime.UtcNow);
                    break;

                case Domain.Common.Enum.TaskStatus.ReworkRequired:
                    task.RequestRedo(currentUserId, "Researcher requested redo");
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Không hỗ trợ chuyển sang trạng thái {nextStatus}");
            }
        }
    }
}
