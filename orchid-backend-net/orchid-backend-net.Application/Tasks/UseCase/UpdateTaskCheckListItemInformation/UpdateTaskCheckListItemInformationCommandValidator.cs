using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.UpdateTaskCheckListItemInformation
{
    public class UpdateTaskCheckListItemInformationCommandValidator : AbstractValidator<UpdateTaskCheckListItemInformationCommand>
    {
        public UpdateTaskCheckListItemInformationCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("TaskId không được để trống");
            RuleFor(x => x.CheckListItemId).NotEmpty().WithMessage("CheckListItemId không được để trống");
        }
    }
}
