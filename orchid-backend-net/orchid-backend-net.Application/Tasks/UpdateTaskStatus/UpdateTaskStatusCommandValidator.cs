using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Task ID is required.");
            RuleFor(x => x.Status)
                .InclusiveBetween(0, 5).WithMessage("Status must be between 0 and 5.");
        }
    }
}
