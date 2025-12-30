using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class TaskAttributes : BaseGuidEntity
    {
        public int? ChemicalId { get; set; }
        public int? MaterialId { get; set; }
        public string TaskId { get; set; }
        [ForeignKey(nameof(ChemicalId))]
        public virtual Chemicals Chemicals { get; set; }
        [ForeignKey(nameof(MaterialId))]
        public virtual Materials Materials { get; set; }
        [ForeignKey(nameof(TaskId))]
        public virtual Tasks Tasks { get; set; }
        public required string Unit { get; set; }
        public required decimal Value { get; set; }
    }
}