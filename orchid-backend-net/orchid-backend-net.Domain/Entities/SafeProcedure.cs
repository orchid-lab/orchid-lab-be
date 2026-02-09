using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class SafeProcedure : AuditableEntity
    {
        public string ProcedureName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ProcedureType { get; set; } = default!;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<SafeProcedureStep> SafeProcedureSteps { get; set; } = new();
    }
}
