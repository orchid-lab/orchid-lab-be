using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
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
        public DateOnly StartedAt { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public virtual List<MonitoringLogs> MonitoringLogs { get; set; }
        public SampleStatus Status { get; set; }
        //0 - Mới tạo - technician chưa nhận experiment log để tiến hành lai tạo
        //1 - Đang tiến hành - diễn ra khi technician nhận experiment log
        //2 - Hoàn thành
        //3 - Bị hủy 
        internal void MarkAsCompleted()
        {
            EnsureNotTerminated();
            if (Status == SampleStatus.Completed)
                return;
            if (Status != SampleStatus.InProgressed)
                throw new DomainException("Stage không ở trạng thái đang tiến hành.");
            Status = SampleStatus.Completed;
        }

        internal void Start()
        {
            if (Status != SampleStatus.Created)
                throw new DomainException("Stage không hợp lệ để bắt đầu.");

            StartedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = SampleStatus.InProgressed;
        }

        internal void MarkAsExecuted()
        {
            EnsureNotTerminated();

            CompletedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = SampleStatus.ExecutedBecauseOfDisease;
        }

        internal void EnsureNotTerminated()
        {
            if (Status == SampleStatus.ExecutedBecauseOfDisease)
                throw new DomainException("Sample này đã hủy do bệnh.");
        }
    }
}
