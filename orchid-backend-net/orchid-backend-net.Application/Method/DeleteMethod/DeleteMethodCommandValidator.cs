using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.DeleteMethod
{
    public class DeleteMethodCommandValidator : AbstractValidator<DeleteMethodCommand>
    {
        private readonly IMethodRepository _methodRepository;
        public DeleteMethodCommandValidator(IMethodRepository methodRepository)
        {
            _methodRepository = methodRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("ID can not null.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsMethodExists(id, cancellationToken))
                .WithMessage(x => $"Not found any Method with ID:{x.ID} in the system");
        }
        private async Task<bool> IsMethodExists(string id, CancellationToken cancellationToken)
        {
            return await _methodRepository.AnyAsync(x => x.ID.Equals(id) && x.Status == true, cancellationToken);
        }
    }
}
