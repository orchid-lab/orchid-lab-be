using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.CreateSeedling
{
    public class CreateSeedlingCommandValidator : AbstractValidator<CreateSeedlingCommand>
    {
        private readonly ISeedlingRepository seedlingRepository;
        public CreateSeedlingCommandValidator(ISeedlingRepository seedlingRepository, ISeedlingAttributeRepository seedlingAttributeRepository)
        {
            this.seedlingRepository = seedlingRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.LocalName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.LocalName)
                //Please fucking remember when the must async has false in it, it will run the fucking message
                //which means, when the function returns true, it will throw out the message
                .MustAsync(async (name, cancellation) => !await IsDuplicateName(name))
                .WithMessage("A seedling with this name already exists.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
            RuleFor(x => x.DoB)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of Birth cannot be in the future.");
            RuleForEach(x => x.Characteristics)
                .SetValidator(new CharacteristicsDTOValidator());
        }

        private async Task<bool> IsDuplicateName(string name)
        {
            return await seedlingRepository.AnyAsync(x => x.LocalName.ToLower().Equals(name.ToLower()));
        }
    }
}
