namespace orchid_backend_net.Application.Common.Extension
{
    public static class ExperimentLogStatusDisplayExtension
    {
        public static string ToDisplayText(this Domain.Common.Enum.ExperimentLogStatus experimentLogStatus)
            => experimentLogStatus switch
            {
                Domain.Common.Enum.ExperimentLogStatus.Created => "mới tạo",
                Domain.Common.Enum.ExperimentLogStatus.InProgress => "đang trong quá trình thực hiện",
                Domain.Common.Enum.ExperimentLogStatus.Destroyed => "bị hủy",
                Domain.Common.Enum.ExperimentLogStatus.WaitingForChangeStage => "chờ thay đổi giai đoạn",
                Domain.Common.Enum.ExperimentLogStatus.ConfirmChangeStage => "xác nhận thay đổi giai đoạn",
                Domain.Common.Enum.ExperimentLogStatus.Completed => "hoàn thành",
                _ => throw new NotImplementedException()
            };
    }
}
