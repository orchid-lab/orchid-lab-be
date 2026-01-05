using FluentValidation;

namespace orchid_backend_net.Application.Tasks.UseCase.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Tên task không được để trống.");
            RuleFor(x => x.CreateTaskAssignment.ExpectedEndDate)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ngày dự kiến kết thúc không được để trống.");
        }
    }
}
