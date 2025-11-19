using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.UpdateSeedling
{
    public class UpdateSeedlingCommandValidator : AbstractValidator<UpdateSeedlingCommand>
    {
        private readonly ISeedlingRepository _seedlingRepository;
        public UpdateSeedlingCommandValidator(ISeedlingRepository seedlingRepository)
        {
            _seedlingRepository = seedlingRepository;
            Configure();
        }
        private void Configure()
        {
            RuleFor(x => x.SeedlingId)
                .NotEmpty().WithMessage("Seedling ID is required.")
                .MaximumLength(50).WithMessage("Seedling ID must not exceed 50 characters.");
            RuleFor(x => x.SeedlingId)
                .MustAsync(async (seedlingId, cancellation) => await IsSeedlingExists(seedlingId))
                .WithMessage("Seedling with this ID does not exist.");
        }

        private async Task<bool> IsSeedlingExists(string seedlingId)
        {
            return await _seedlingRepository.AnyAsync(x => x.ID.Equals(seedlingId));
        }
    }
}
