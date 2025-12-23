using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class Tasks : BaseGuidEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //StageId để xác định là cái task này có phải là template hay không
        public string? StageId { get; set; }
        public string? ResearcherId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Status { get; set; }
        //0 - Chưa nhận
        //1 - Đang tiến hành
        //3 - Đang chờ xác nhận đã hoàn thành từ Researcher
        //4 - Đã hoàn thành
        public virtual IEnumerable<TaskAssignment> TaskAssignments { get; set; } = [];
        public virtual IEnumerable<TaskAttributes> TaskAttributes { get; set; } = [];
    }
}
