namespace orchid_backend_net.Application.Common.Extension
{
    public static class TaskStatusDisplayExtension
    {
        public static string ToDisplayText(this Domain.Common.Enum.TaskStatus status)
        {
            return status switch
            {
                Domain.Common.Enum.TaskStatus.Assigned => " technician chưa nhận.",
                Domain.Common.Enum.TaskStatus.InProgress => "đang tiến hành.",
                Domain.Common.Enum.TaskStatus.WaitingForApproval => "đang chờ xác nhận đã hoàn thành từ Researcher.",
                Domain.Common.Enum.TaskStatus.CompletedInTime => "đã hoàn thành đúng hạn.",
                Domain.Common.Enum.TaskStatus.CompletedOutTime => "đã hoàn thành trễ hạn.",
                Domain.Common.Enum.TaskStatus.Deleted => "đã bị xoá.",
                Domain.Common.Enum.TaskStatus.DeclinedByTechnician => "đã bị technician từ chối.",
                Domain.Common.Enum.TaskStatus.ReworkRequired => "yêu cầu làm lại.",
                _ => "không xác định."
            };
        }
    }
}
