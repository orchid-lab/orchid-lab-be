namespace orchid_backend_net.Domain.Common.Enum
{
    public enum TaskStatus
    {
        //task vừa được tạo xong
        Created,
        //technician nhận task để làm
        InProgress,
        //technician hoàn thành xong task, chờ approval từ researcher
        WaitingForApproval,
        //researcher đã approve task
        //nếu researcher không approve thì task sẽ trở về trạng thái InProgress
        CompletedInTime,
        CompletedOutTime,
        //researcher xóa task do lỗi hoặc không cần thiết
        Deleted
    }
}
