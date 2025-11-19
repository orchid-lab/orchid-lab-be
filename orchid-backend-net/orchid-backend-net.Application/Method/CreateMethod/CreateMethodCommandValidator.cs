using FluentValidation;
using orchid_backend_net.Domain.Enums;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.CreateMethod
{
    public class CreateMethodCommandValidator : AbstractValidator<CreateMethodCommand>
    {
        private readonly IMethodRepository _methodRepository;

        public CreateMethodCommandValidator(IMethodRepository methodRepository)
        {
            _methodRepository = methodRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name can not be null.");
            //func return true => passed the validation
            //func return false => failed the validation
            RuleFor(x => x.Name)
                .MustAsync(async (name, cancellationToken) => !await IsNameDuplicated(name))
                .WithMessage("A method with this name already exists");
            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull()
                .WithMessage("Description can not be null.");
            RuleFor(x => x.Name.Length)
                .LessThanOrEqualTo(50)
                .WithMessage("Name is too long");
            RuleFor(x => x.Description.Length)
                .LessThanOrEqualTo(500)
                .WithMessage("Description is too long.");
            RuleFor(x => x.Type)
                .Must((methodType) => IsDefinedInEnum(methodType))
                .WithMessage("Type of Method must be Clonel or Sexual propagation");
            RuleFor(x => x.Type)
                .LessThanOrEqualTo(2)
                .GreaterThanOrEqualTo(0);
        }
        async Task<bool> IsNameDuplicated(string name)
        {
            return await this._methodRepository.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }
        bool IsDefinedInEnum(int methodType)
            => Enum.IsDefined(typeof(MethodType), methodType);
    }
}
