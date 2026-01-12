namespace orchid_backend_net.Application.Common.Extension
{
    public static class ExperimentLogStatusDisplayExtension
    {
        public static string ToDisplayText(this Domain.Common.Enum.ExperimentLogStatus experimentLogStatus)
            => experimentLogStatus switch
            {
                Domain.Common.Enum.ExperimentLogStatus.Created => "mới tạo",
                Domain.Common.Enum.ExperimentLogStatus.InProgessed => "đang trong quá trình thực hiện",
                Domain.Common.Enum.ExperimentLogStatus.Destroyed => "bị hủy",
                _ => throw new NotImplementedException()
            };
    }
}
