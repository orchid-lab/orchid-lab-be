using FluentValidation;

namespace orchid_backend_net.Application.TaskAssign.DeleteTaskAssign
{
    public class DeleteTaskAssignCommandValidator : AbstractValidator<DeleteTaskAssignCommand>
    {
        public DeleteTaskAssignCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.TaskId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Task cannot be null or empty.");
        }
    }
}
