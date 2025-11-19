using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.GetSampleInfor
{
    public class GetSampleInforQueryValidator : AbstractValidator<GetSampleInforQuery>
    {
        private readonly ISampleRepository _sampleRepository;
        public GetSampleInforQueryValidator(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.ID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Sample ID can not empty.");
            RuleFor(x => x.ID)
                .MustAsync(async (id, cancellationToken) => await IsSampleExist(id, cancellationToken))
                .WithMessage(x => $"Not found sample with ID:{x.ID} in the system.");
        }
        private async Task<bool> IsSampleExist(string id, CancellationToken cancellationToken)
            => await _sampleRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
