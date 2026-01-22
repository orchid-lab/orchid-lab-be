namespace orchid_backend_net.Application.Common.Extension
{
    public static class BatchStatusDisplayExtension
    {
        public static string ToDisplayText(this Domain.Common.Enum.BatchStatus status)
        {
            return status switch
            {
                Domain.Common.Enum.BatchStatus.Ready => "Sẵn sàng chờ thí nghiệm",
                Domain.Common.Enum.BatchStatus.InUse => "Đang tiến hành thí nghiệm",
                Domain.Common.Enum.BatchStatus.Cleaning => "Đang vệ sinh",
                Domain.Common.Enum.BatchStatus.Maintenance => "Đã Bảo trì",
                Domain.Common.Enum.BatchStatus.Inactive => "Không hoạt động",
                _ => "Trạng thái không xác định",
            };
        }
    }
}
