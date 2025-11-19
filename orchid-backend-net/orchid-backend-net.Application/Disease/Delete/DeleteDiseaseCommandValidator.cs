using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Delete
{
    public class DeleteDiseaseCommandValidator : AbstractValidator<DeleteDiseaseCommand>
    {
        private readonly IDiseaseRepository _diseaseRepository;
        public DeleteDiseaseCommandValidator(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Disease ID must not be empty.")
                .MustAsync(async (id, cancellationToken) => await IsDiseaseExists(id, cancellationToken))
                .WithMessage("Disease does not exist");
        }

        private async Task<bool> IsDiseaseExists(string id, CancellationToken cancellationToken)
            => await _diseaseRepository.AnyAsync(d => d.ID.Equals(id), cancellationToken);
    }
}
