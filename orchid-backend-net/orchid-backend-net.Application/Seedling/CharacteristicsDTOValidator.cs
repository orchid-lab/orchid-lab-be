using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling
{
    public class CharacteristicsDTOValidator : AbstractValidator<CharacteristicsDTO>
    {
        public CharacteristicsDTOValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required.");
            RuleFor(x => x.SeedlingAttribute).
                SetValidator(new SeedlingAttributeDTOValidator());
        }
    }
}
