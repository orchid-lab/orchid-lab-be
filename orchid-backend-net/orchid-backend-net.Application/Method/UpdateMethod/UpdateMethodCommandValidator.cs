using FluentValidation;
using orchid_backend_net.Domain.Enums;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UpdateMethod
{
    public class UpdateMethodCommandValidator : AbstractValidator<UpdateMethodCommand>
    {
        private readonly IMethodRepository _methodRepository;
        public UpdateMethodCommandValidator(IMethodRepository methodRepository)
        {
            Configuration();
            _methodRepository = methodRepository;
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID is required.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsExistMethod(id, cancellationToken))
                .WithMessage("Method does not exist.");
            RuleFor(x => x.Type)
               .Must((methodType) => IsDefinedInEnum(methodType))
               .WithMessage("Type of Method must be Clonel or Sexual propagation");
            RuleFor(x => x.Type)
                .LessThanOrEqualTo(2)
                .GreaterThanOrEqualTo(1);
        }
        private async Task<bool> IsExistMethod(string id, CancellationToken cancellationToken)
        {
           return await _methodRepository.AnyAsync(x => x.ID.Equals(id) && x.Status, cancellationToken);
        }
        bool IsDefinedInEnum(int? methodType)
        {
            if (methodType == 0 || methodType == null)
                return true;
            return Enum.IsDefined(typeof(MethodType), methodType);
        }
    }
}
