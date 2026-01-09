using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class ExperimentLogs : BaseGuidEntity
    {
        public required string HybridzationId { get; set; }
        public virtual Hybridzations Hybridzations { get; set; }
        public required int MethodId { get; set; }
        [ForeignKey(nameof(MethodId))]
        public virtual Methods Method { get; set; }
        public required int BatchId { get; set; }
        [ForeignKey(nameof(BatchId))]
        public virtual Batches Batch { get; set; }
        public required string Name { get; set; }
        public required string CreatedBy { get; set; }
        public required string AssignedTo { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Reason { get; set; }
        public ExperimentLogStatus Status { get; set; }
        //0 - Mới tạo - chưa nhận
        //1 - đang tiến hành - diễn ra khi technician nhận experiment log
        //2 - Hoàn thành
        //3 - Bị hủy => hủy toàn bộ samples thuộc experiment log này
        public virtual List<Samples> Samples { get; set; } = [];

        public void UpdateInformation(string? name, string? notes)
        {
            Name = name ?? Name;
            Notes = notes;
        }

        public void MarkCompleted()
        {
            EnsureNotDestroyed();

            if (Status == ExperimentLogStatus.Completed)
                return;
            Status = ExperimentLogStatus.Completed;
        }

        public void DestroyExperimentLogBecauseOfAllSampleInfected(string? reason)
        {
            EnsureNotDestroyed();
            Reason = reason;
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = ExperimentLogStatus.Destroyed;
        }

        private void EnsureNotDestroyed()
        {
            if(Status == ExperimentLogStatus.Destroyed)
                throw new DomainException("Experiment log đã bị hủy.");

        }
    }
}