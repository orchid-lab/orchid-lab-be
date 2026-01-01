using FluentValidation;
using orchid_backend_net.Application.Common.Helper;

namespace orchid_backend_net.Application.Tasks.CreateTask
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
            RuleFor(x => x.ExpectedEndDate)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ngày kết thúc không được để trống.");
        }
    }
}
