using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.TechnicianSubmitTaskCheckList
{
    public class TechnicianSubmitTaskCheckListItemCommandValidator : AbstractValidator<TechnicianSubmitTaskCheckListItemCommand>
    {
        public TechnicianSubmitTaskCheckListItemCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("TaskId không được để trống");
            RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId không được để trống");
        }
    }
}
