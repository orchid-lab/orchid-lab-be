namespace orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep
{
    public class CreateSafeProcedureStepDto
    {
        public string SafeProcedureStepName { get; set; } = default!;
        public int StepNumber { get; set; }
        public string Description { get; set; } = default!;
    }
}
