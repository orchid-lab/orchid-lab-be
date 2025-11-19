using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Update
{
    public class UpdateDiseaseCommandValidator : AbstractValidator<UpdateDiseaseCommand>
    {
        private readonly IDiseaseRepository _diseaseRepository;
        public UpdateDiseaseCommandValidator(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
            RuleFor(x => x.Id)
                .NotNull().NotEmpty().WithMessage("Disease ID is required.")
                .MustAsync(async (id, cancellationToken) => await IsExist(id, cancellationToken))
                .WithMessage("Disease with the specified ID does not exist.");
        }
        private async Task<bool> IsExist(string id, CancellationToken cancellationToken)
            => await _diseaseRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
