using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.GetInfor
{
    public class GetDiseaseInforQueryValidator : AbstractValidator<GetDiseaseInforQuery>
    {
        private readonly IDiseaseRepository _diseaseRepository;
        public GetDiseaseInforQueryValidator(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Disease ID is required.")
                .MustAsync(async (id, cancellation) => await IsDiseaseExists(id, cancellation))
                .WithMessage("Disease with the specified ID does not exist.");
        }

        private async Task<bool> IsDiseaseExists(string id, CancellationToken cancellationToken)
        {
            return await _diseaseRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        }
    }
}
