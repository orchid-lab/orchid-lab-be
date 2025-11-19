using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.InfectedSample.CreateInfectedSampleCommand
{
    public class CreateInfectedSampleCommandValidator : AbstractValidator<CreateInfectedSampleCommand>
    {
        private readonly IDiseaseRepository _diseaseRepository;
        public CreateInfectedSampleCommandValidator(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.SampleID).NotEmpty().WithMessage("SampleID is required.");
            RuleFor(x => x.DiseaseID).NotEmpty().WithMessage("DiseaseID is required.");
            RuleFor(x => x.DiseaseID).MustAsync(async (id, cancellationToken) => await BeAValidDiseaseId(id, cancellationToken)).WithMessage("DiseaseID is not valid.");
        }

        private async Task<bool> BeAValidDiseaseId(string diseaseId, CancellationToken cancellationToken)
        {
            var disease = await _diseaseRepository.FindAsync(x => x.ID.Equals(diseaseId), cancellationToken);
            return disease != null;
        }
    }
}
