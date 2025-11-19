using FluentValidation;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Sample.ConvertToSeedling
{
    public class ConvertToSeedlingCommandValidator : AbstractValidator<ConvertToSeedlingCommand>
    {
        private readonly ISampleRepository _sampleRepository;
        public ConvertToSeedlingCommandValidator(ISampleRepository sampleRepository) 
        {
            this._sampleRepository = sampleRepository;
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.SampleID)
                .NotEmpty()
                .NotNull()
                .WithMessage("Sample ID can't nul or empty.");
            RuleFor(x => x.SampleID)
                .MustAsync(async (id, cancellationToken) => await IsExsitSample(id, cancellationToken))
                .WithMessage("Not found sample with ID.");
            RuleFor(x => x.SampleID)
                .MustAsync(async (id, cancellationToken) => await IsDoneInHealthy(id, cancellationToken))
                .WithMessage("Sample is not ready to convert to an seedling.");
        }
        async Task<bool> IsExsitSample(string id, CancellationToken cancellationToken)
            => await this._sampleRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
        async Task<bool> IsDoneInHealthy(string id, CancellationToken cancellationToken)
            => await this._sampleRepository.AnyAsync(x => x.ID.Equals(id) && x.Linkeds.Any(x => x.ProcessStatus == 2) && x.Status == 0, cancellationToken);
    }
}
