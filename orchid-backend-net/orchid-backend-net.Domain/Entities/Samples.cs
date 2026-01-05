using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class Samples : BaseGuidEntity
    {
        public string Name { get; set; } = null!;
        public required string ExperimentLogId { get; set; }
        public int CurrentStageOrder { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public DateOnly ExecutionDate { get; set; }
        public SampleStatus Status { get; set; }
        //0 - Mới tạo - technician chưa nhận experiment log để tiến hành lai tạo
        //1 - Đang tiến hành - diễn ra khi technician nhận experiment log
        //2 - Hoàn thành
        //3 - Bị hủy 
        [ForeignKey(nameof(ExperimentLogId))]
        public virtual ExperimentLogs ExperimentLog { get; set; } = null!;
        public virtual IEnumerable<TaskAssignment> TaskAssignments { get; set; } = [ ];
    }
}