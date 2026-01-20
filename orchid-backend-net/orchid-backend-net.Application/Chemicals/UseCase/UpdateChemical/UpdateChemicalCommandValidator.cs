using FluentValidation;
using orchid_backend_net.Application.Chemicals.Policy;

namespace orchid_backend_net.Application.Chemicals.UseCase.UpdateChemical
{
    public class UpdateChemicalCommandValidator : AbstractValidator<UpdateChemicalCommand>
    {
        public UpdateChemicalCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(command => !string.IsNullOrEmpty(command.Name))
                .WithMessage("Tên phải dưới 100 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(command => !string.IsNullOrEmpty(command.Description))
                .WithMessage("Chú thích phải dưới 500 ký tự");
            RuleFor(x => x.Category)
                .Must(category => category == null || ChemicalPolicy.IsValidCategory(category))
                .WithMessage("Loại không hợp lệ");
            RuleFor(x => x.Unit)
                .Must(unit => unit == null || ChemicalPolicy.IsValidUnit(unit))
                .WithMessage("Đơn vị không hợp lệ");
        }
    }
}
