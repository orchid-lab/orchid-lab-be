namespace orchid_backend_net.Domain.Common.Enum
{
    public enum ExperimentLogStatus
    {
        //Created: đã tạo, đã assign technician, CHƯA bắt đầu thực nghiệm
        Created,
        //InProgressed: đang thực hiện các stage
        InProgressed,
        WaitingForChangeState,
        //Completed: hoàn thành
        Completed,
        //Destroyed: hủy do toàn bộ sample nhiễm bệnh
        Destroyed,
    }
}
