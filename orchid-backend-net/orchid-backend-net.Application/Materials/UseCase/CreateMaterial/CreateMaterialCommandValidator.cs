using FluentValidation;
using orchid_backend_net.Application.Materials.Policy;

namespace orchid_backend_net.Application.Materials.UseCase.CreateMaterial
{
    public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
    {
        public CreateMaterialCommandValidator() 
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên phải dưới 100 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Chú thích phải dưới 500 ký tự");

            RuleFor(x => x.Category)
                .NotEmpty()
                .NotNull()
                .WithMessage("Loại không được để trống");
            RuleFor(x => x.Category)
                .Must(category => MaterialPolicy.IsValidCategory(category))
                .WithMessage("Loại không hợp lệ");

            RuleFor(x => x.Unit)
                .NotNull()
                .NotEmpty()
                .WithMessage("Đơn vị không được để trống");
            RuleFor(x => x.Unit)
                .Must(unit => MaterialPolicy.IsValidUnit(unit))
                .WithMessage("Đơn vị không hợp lệ");
        }
    }
}
