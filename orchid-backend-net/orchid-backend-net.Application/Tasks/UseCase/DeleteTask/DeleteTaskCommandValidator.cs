using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.DeleteTask
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>  
    {
        public DeleteTaskCommandValidator() 
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.TaskId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Task Id không được để trống");
        }
    }
}
