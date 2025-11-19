using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.DeleteSample
{
    public class DeleteSampleCommandValidator : AbstractValidator<DeleteSampleCommand>
    {
        private readonly ISampleRepository _sampleRepository;
        public DeleteSampleCommandValidator(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Sample ID is required.");
            RuleFor(x => x.Reason).NotEmpty().WithMessage("Reason for deletion is required.");
            RuleFor(x => x.DiseaseId).NotEmpty().WithMessage("Disease ID is required.");
            RuleFor(x => x.DiseaseId).MustAsync(async (sampleId, cancellationToken) => await SampleExists(sampleId, cancellationToken))
                .WithMessage("Sample with the given ID does not exist.");
        }

        private async Task<bool> SampleExists(string sampleId, CancellationToken cancellationToken)
        {
            var sample = await _sampleRepository.FindAsync(x => x.ID.Equals(sampleId), cancellationToken);
            return sample != null;
        }
    }
}
