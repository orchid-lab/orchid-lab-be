using FluentValidation;

namespace orchid_backend_net.Application.LabConfig.UseCase.Create
{
    public class CreateLabConfigCommandValidator : AbstractValidator<CreateLabConfigCommand>
    {
        public CreateLabConfigCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.ConfigName)
                .NotEmpty().WithMessage("Tên là bắt buộc.")
                .MaximumLength(100).WithMessage("Tên phải dưới 100 ký tự.");
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key là bắt buộc.")
                .MaximumLength(100).WithMessage("Key phải dưới 100 ký tự.");
            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("Giá trị phải lớn hơn 0.");
        }
    }
}
