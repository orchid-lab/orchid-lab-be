using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.ResearcherEvaluateTaskCheckList
{
    public class ResearcherEvaluateTaskChecklistItemValidator : AbstractValidator<ResearcherEvaluateTaskCheckListCommand>
    {
        public ResearcherEvaluateTaskChecklistItemValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("TaskId không được để trống");
            RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId không được để trống");
            RuleFor(x => x.IsPass).NotNull().WithMessage("IsPass không được để trống");
        }
    }
}
