using FluentValidation;

namespace orchid_backend_net.Application.TaskAttribute.DeleteTaskAttribute
{
    public class DeleteTaskAttributeCommandValidator : AbstractValidator<DeleteTaskAttributeCommand>
    {
        public DeleteTaskAttributeCommandValidator() 
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
