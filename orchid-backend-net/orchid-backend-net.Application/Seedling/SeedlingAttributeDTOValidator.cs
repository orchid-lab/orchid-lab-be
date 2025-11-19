using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling
{
    public class SeedlingAttributeDTOValidator : AbstractValidator<SeedlingAttributeDTO>
    {
        public SeedlingAttributeDTOValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
