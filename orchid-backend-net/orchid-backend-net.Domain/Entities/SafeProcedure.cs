using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities.Base;

namespace orchid_backend_net.Domain.Entities
{
    public class SafeProcedure : AuditableEntity
    {
        public string ProcedureName { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string ProcedureType { get; set; } = default!;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual List<SafeProcedureStep> SafeProcedureSteps { get; set; } = new();

        public void AddStep(string Name, int Step, string? Description)
        {
            var isDuplicatedStep = SafeProcedureSteps.Any(s => s.StepNumber == Step);
            if(isDuplicatedStep)
            {
                throw new DuplicateException($"Step {Step} đã tồn tại trong quy trình.");
            }


            var isDuplicatedName = SafeProcedureSteps.Any(s => s.SafeProcedureStepName == Name);
            if(isDuplicatedName)
            {
                throw new DuplicateException($"Tên bước '{Name}' đã tồn tại trong quy trình.");
            }

            var newStep = new SafeProcedureStep
            {
                SafeProcedureStepName = Name,
                StepNumber = Step,
                Description = Description,
            };

            SafeProcedureSteps.Add(newStep);
        }

        public void UpdateStep(string StepId, string? Name, int Step, string? Description)
        {
            var stepToUpdate = SafeProcedureSteps.FirstOrDefault(s => s.ID == StepId)
                ?? throw new NotFoundException($"Không tìm thấy bước với ID {StepId}.");
            
            var isDuplicatedStep = SafeProcedureSteps.Any(s => s.StepNumber == Step && s.ID != StepId);
            if(isDuplicatedStep)
            {
                throw new DuplicateException($"Step {Step} đã tồn tại trong quy trình.");
            }
            var isDuplicatedName = SafeProcedureSteps.Any(s => s.SafeProcedureStepName == Name && s.ID != StepId);
            if(isDuplicatedName)
            {
                throw new DuplicateException($"Tên bước '{Name}' đã tồn tại trong quy trình.");
            }
            stepToUpdate.SafeProcedureStepName = Name ?? stepToUpdate.SafeProcedureStepName;
            stepToUpdate.StepNumber = Step;
            stepToUpdate.Description = Description ?? stepToUpdate.Description;
        }

        public void RemoveStep(string StepId)
        {
            var stepToRemove = SafeProcedureSteps.FirstOrDefault(s => s.ID == StepId)
                ?? throw new NotFoundException($"Không tìm thấy bước với ID {StepId}.");
            SafeProcedureSteps.Remove(stepToRemove);
        }
    }
}
