using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Disease.Create
{
    public class CreateDiseaseCommandValidator : AbstractValidator<CreateDiseaseCommand>
    {
        private readonly IDiseaseRepository _diseaseRepository;
        public CreateDiseaseCommandValidator(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required.")
               .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
               .MustAsync(async (name, cancellationToken) => !await IsNameUnique(name, cancellationToken))
               .WithMessage("A disease with this name already exists.");
            RuleFor(x => x.Description)
                .NotNull().NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Solution)
                .NotNull().NotEmpty().WithMessage("Solution is required.");
            RuleFor(x => x.InfectedRate)
                .GreaterThanOrEqualTo(0).WithMessage("Infected rate must be a non-negative number.");
        }

        private async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
            => await _diseaseRepository.AnyAsync(d => d.Name.ToLower().Equals(name.ToLower()), cancellationToken);
    }
}
