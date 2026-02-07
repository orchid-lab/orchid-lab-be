using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class SafeProcedure : BaseGuidEntity
    {
        public string ProcedureName { get; set; } = default!;
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
    }
}
