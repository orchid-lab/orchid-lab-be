using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SampleStage : BaseGuidEntity
    {
        public string SampleId { get; set; }
        [ForeignKey(nameof(SampleId))]
        public virtual Samples Samples { get; set; }
        public int SampleStageDefinitionId { get; set; }
        [ForeignKey(nameof(SampleStageDefinitionId))]
        public virtual SampleStageDefinition SampleStageDefinition { get; set; }
        public SampleStatus Status { get; set; }
        //0 - Mới tạo - technician chưa nhận experiment log để tiến hành lai tạo
        //1 - Đang tiến hành - diễn ra khi technician nhận experiment log
        //2 - Hoàn thành
        //3 - Bị hủy 
    }
}
