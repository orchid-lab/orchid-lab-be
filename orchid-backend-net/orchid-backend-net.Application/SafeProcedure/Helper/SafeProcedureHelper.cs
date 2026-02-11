using orchid_backend_net.Application.SafeProcedure.Dto.SafeProcedureStep;

namespace orchid_backend_net.Application.SafeProcedure.Helper
{
    public static class SafeProcedureHelper
    {
        public static void AddStepsToSafeProcedure(Domain.Entities.SafeProcedure safeProcedure, List<CreateSafeProcedureStepDto> steps)
        {
            if (steps is null)
                return;
            foreach (var step in steps)
            {
                safeProcedure.AddStep(step.SafeProcedureStepName, step.StepNumber, step.Description);
            }
        }
        public static void UpdateStepsOfSafeProcedure(Domain.Entities.SafeProcedure safeProcedure, List<UpdateSafeProcedureStepDto>? steps)
        {
            if (steps is null)
                return;
            foreach (var step in steps)
            {
                safeProcedure.UpdateStep(step.Id, step.SafeProcedureStepName, step.Step, step.Description);
            }
        }
    }
}
