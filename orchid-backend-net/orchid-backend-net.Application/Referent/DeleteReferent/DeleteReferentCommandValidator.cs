using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Referent.DeleteReferent
{
    public class DeleteReferentCommandValidator : AbstractValidator<DeleteReferentCommand>
    {
        private readonly IReferentRepository _referentRepository;
        public DeleteReferentCommandValidator(IReferentRepository referentRepository)
        {
            _referentRepository = referentRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotNull().WithMessage("Id cannot be null.")
                .MustAsync(async(id, cancellationToken) => await IsReferentExists(id, cancellationToken))
                .WithMessage(x => $"Not found Referent with id: {x.Id}");
        }

        private async Task<bool> IsReferentExists(string id, CancellationToken cancellationToken)
        {
            return await _referentRepository.AnyAsync(x => x.ID.Equals(id) && x.Status, cancellationToken);
        }
    }
}
