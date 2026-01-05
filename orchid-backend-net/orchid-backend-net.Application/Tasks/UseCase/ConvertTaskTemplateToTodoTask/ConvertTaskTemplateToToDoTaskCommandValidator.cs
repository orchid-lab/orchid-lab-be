using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.ConvertTaskTemplateToTodoTask
{
    public class ConvertTaskTemplateToToDoTaskCommandValidator : AbstractValidator<ConvertTaskTemplateToToDoTaskCommand>
    {
        public ConvertTaskTemplateToToDoTaskCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.TaskTemplateId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Id của mẫu công việc không được để trống.");
        }
    }
}
