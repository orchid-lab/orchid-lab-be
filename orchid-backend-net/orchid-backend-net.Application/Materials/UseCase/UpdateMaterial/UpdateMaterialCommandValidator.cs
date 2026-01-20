using FluentValidation;
using orchid_backend_net.Application.Materials.Policy;

namespace orchid_backend_net.Application.Materials.UseCase.UpdateMaterial
{
    public class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
    {
        public UpdateMaterialCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("ID phải lớn hơn 0");
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(command => !string.IsNullOrEmpty(command.Name))
                .WithMessage("Tên phải dưới 100 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(command => !string.IsNullOrEmpty(command.Description))
                .WithMessage("Chú thích phải dưới 500 ký tự");
            RuleFor(x => x.Category)
                .Must(category => category == null || MaterialPolicy.IsValidCategory(category))
                .WithMessage("Loại không hợp lệ");
            RuleFor(x => x.Unit)
                .Must(unit => unit == null || MaterialPolicy.IsValidUnit(unit))
                .WithMessage("Đơn vị không hợp lệ");
        }
    }
}
