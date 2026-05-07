using FluentValidation;
namespace orchid_backend_net.Application.MethodStageDefinition.UseCase.CreateMethodStageDefinition
{
    public class CreateMethodStageDefinitionCommandValidator : AbstractValidator<CreateMethodStageDefinitionCommand>
    {
        public CreateMethodStageDefinitionCommandValidator()
        {
            Configuration();
        }
        private void Configuration()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên phải dưới 100 ký tự");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Chú thích phải dưới 500 ký tự");
        }
    }
}
