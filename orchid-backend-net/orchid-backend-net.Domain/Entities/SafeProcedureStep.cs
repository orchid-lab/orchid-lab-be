using orchid_backend_net.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace orchid_backend_net.Domain.Entities
{
    public class SafeProcedureStep : BaseGuidEntity
    {
        public string SafeProcedureId { get; set; } = default!;
        [ForeignKey(nameof(SafeProcedureId))]
        public virtual SafeProcedure SafeProcedure { get; set; } = default!;
        public string SafeProcedureStepName { get; set; } = default!;
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
    }
}
