namespace orchid_backend_net.Domain.Common.Enum
{
    public enum TaskStatus
    {
        //task vừa được tạo xong
        Assigned,
        //technician nhận task để làm
        InProgress,
        //technician hoàn thành xong task, chờ approval từ researcher
        WaitingForApproval,
        //researcher đã approve task
        //nếu researcher không approve thì task sẽ trở về trạng thái InProgress
        //hoàn thành đúng hạn
        CompletedInTime,
        //hoàn thành trễ hạn
        CompletedOutTime,
        //researcher xóa task do lỗi hoặc không cần thiết
        Deleted,
        //technician từ chối nhận task
        DeclinedByTechnician,
        //researcher yêu cầu làm lại
        ReworkRequired,
        //chỉ là mẫu thôi không có đá đụng gì hết, là template thì phải tạo mới task mới được gán
        Template
    }
}
