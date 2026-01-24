namespace orchid_backend_net.Domain.Common.Enum
{
    public enum BatchStatus
    {
        //sẵn sàng cho việc sử dụng để thí nghiệm
        Ready,
        //đang được sử dụng để thí nghiệm
        InUse,
        //đang được làm sạch sau thí nghiệm
        Cleaning,
        //đang bảo trì
        Maintenance,
        //xóa khỏi hệ thống
        Inactive
    }
}
