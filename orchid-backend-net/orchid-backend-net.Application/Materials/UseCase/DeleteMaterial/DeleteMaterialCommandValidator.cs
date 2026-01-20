using FluentValidation;

namespace orchid_backend_net.Application.Materials.UseCase.DeleteMaterial
{
    public class DeleteMaterialCommandValidator : AbstractValidator<DeleteMaterialCommand>
    {
        public DeleteMaterialCommandValidator()
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
        }
    }
}
