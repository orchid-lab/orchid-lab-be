using FluentValidation;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Referent.UpdateReferent
{
    public class UpdateReferentCommandValidator : AbstractValidator<UpdateReferentCommand>
    {
        private readonly IReferentRepository _referentRepository;
        public UpdateReferentCommandValidator(IReferentRepository referentRepository)
        {
            _referentRepository = referentRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id cannot be null or empty.");
            RuleFor(x => x.Unit)
                .Must(unit => ValidateUnit(unit))
                .WithMessage("Unit must be: [cái, mm, cm, g, %]");
            RuleFor(x => x.Unit)
                .Must((unit) => ValidateUnit(unit))
                .WithMessage("Unit must be: [cái, mm, cm, g, %]");
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) => !await IsNameDuplicated(command.Id, command.Name ?? string.Empty, cancellationToken))
                .WithMessage(x => $"Referent with name: {x.Name} has been duplicated.");
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) => await IsValueFromSmallerThanValueTo(command.ValueFrom, command.ValueTo))
                .WithMessage(x => $"Value from: {x.ValueFrom} must be smaller than value to: {x.ValueTo}.");
        }

        private bool ValidateUnit(string? unit)
        {
            if(!string.IsNullOrWhiteSpace(unit))
            {
                var validUnits = new List<string> { "cái", "mm", "cm", "g", "%" };
                return validUnits.Contains(unit);
            }
            return true; // If unit is null or empty, we consider it valid
        }
        private async Task<bool> IsValueFromSmallerThanValueTo(decimal? valueFrom, decimal? valueTo)
        {
            if (valueFrom.HasValue && valueTo.HasValue)
            {
                return valueFrom.Value < valueTo.Value;
            }
            return true; // If either value is null, we consider it valid
        }
        private async Task<bool> IsNameDuplicated(string id, string name, CancellationToken cancellationToken)
        {
            if(!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(name))
            {
                var referent = await _referentRepository.FindAsync(x => x.ID.Equals(id) && x.Status, cancellationToken);
                var allReferentsInStage = await _referentRepository.FindAllAsync(x => x.StageID.Equals(referent.StageID) && x.Status, cancellationToken);
                return allReferentsInStage.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !x.ID.Equals(id));
            }
            return true; // If id or name is null or empty, we consider it valid
        }
    }
}
