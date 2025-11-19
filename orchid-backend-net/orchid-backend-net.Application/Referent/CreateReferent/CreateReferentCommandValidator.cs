using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Referent.CreateReferent
{
    public class CreateReferentCommandValidator : AbstractValidator<CreateReferentCommand>
    {
        private readonly IReferentRepository _referentRepository;
        private static readonly string[] AllowedUnits = { "cái", "mm", "cm", "g", "%" };

        public CreateReferentCommandValidator(IReferentRepository referentRepository)
        {
            _referentRepository = referentRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) => !await IsNameDuplicated(command.StageId, command.Name, cancellationToken))
                .WithMessage(x => $"Referent with name: {x.Name} has been duplicated.");
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.");
            RuleFor(x => x.ValueFrom)
                .NotNull()
                .NotEmpty()
                .WithMessage("Value from cannot be null or empty.");
            RuleFor(x => x.ValueTo)
                .NotNull()
                .NotEmpty()
                .WithMessage("Value to cannot be null or empty.");
            RuleFor(x => x.ValueFrom)
                .GreaterThan(x => x.ValueTo)
                .WithMessage("Value from must smaller than value to.");
            RuleFor(x => x.Unit)
                .Must((unit) => ValidateUnit(unit))
                .WithMessage("Unit must be: [cái, mm, cm, g, %]");
        }

        private async Task<bool> IsNameDuplicated(string stageId, string name, CancellationToken cancellationToken)
            => await _referentRepository.AnyAsync(x => x.StageID.Equals(stageId) && x.Name.ToLower().Equals(name.ToLower()) && x.Status, cancellationToken);
        private bool ValidateUnit(string unit) => AllowedUnits.Contains(unit.ToLower());
    }
}
