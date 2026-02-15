using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.RemoveTaskCheckListItem
{
    public class RemoveTaskCheckListItemCommandValidator : AbstractValidator<RemoveTaskCheckListItemCommand>
    {
        public RemoveTaskCheckListItemCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty().WithMessage("TaskId không được để trống");
            RuleFor(x => x.CheckListItemId)
                .NotEmpty().WithMessage("CheckListItemId không được để trống");
        }
    }
}
