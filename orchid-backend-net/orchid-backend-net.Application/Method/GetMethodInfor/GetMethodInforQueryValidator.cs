using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.GetMethodInfor
{
    public class GetMethodInforQueryValidator : AbstractValidator<GetMethodInforQuery>
    {
        private readonly IMethodRepository _methodRepository;
        public GetMethodInforQueryValidator(IMethodRepository methodRepository)
        {
            _methodRepository = methodRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not be null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsMethodExist(id, cancellationToken))
                .WithMessage(x => $"Not found any method with ID : {x.ID}.");
        }
        private async Task<bool> IsMethodExist(string id, CancellationToken cancellationToken)
            => await _methodRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);

    }
}
