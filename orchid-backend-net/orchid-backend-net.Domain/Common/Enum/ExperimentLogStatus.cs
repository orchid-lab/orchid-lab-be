namespace orchid_backend_net.Domain.Common.Enum
{
    public enum ExperimentLogStatus
    {
        //Created: đã tạo, đã assign technician, CHƯA bắt đầu thực nghiệm
        Created,
        //InProgressed: đang thực hiện các stage
        InProgress,
        //WaitingForChangeState: chờ thay đổi trạng thái sau khi hoàn thành một stage của method
        WaitingForChangeStage,
        //ConfỉmChangeStage: đã xác nhận thay đổi stage, chờ technician thực hiện stage tiếp theo
        ConfirmChangeStage,
        //Completed: hoàn thành
        Completed,
        //Destroyed: hủy do toàn bộ sample nhiễm bệnh
        Destroyed,
        //Cancelled: hủy do các nguyên nhân khác trước khi bắt đầu thí nghiệm
        Cancelled
    }
}
