using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.ChangeTaskStatus
{
    public class ChangeTaskStatusCommandValidator : AbstractValidator<ChangeTaskStatusCommand>  
    {
        public ChangeTaskStatusCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.TodoTaskId)
                .NotEmpty()
                .NotEmpty()
                .WithMessage("To-do Task Id không được để trống");
            
            RuleFor(x => x.Status)
                .NotEmpty()
                .NotEmpty()
                .WithMessage("To-do Task Status không được để trống");

        }
    }
}
