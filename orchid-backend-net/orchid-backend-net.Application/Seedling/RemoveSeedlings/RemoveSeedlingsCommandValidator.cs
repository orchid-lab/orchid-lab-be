using FluentValidation;

namespace orchid_backend_net.Application.Seedling.RemoveSeedlings
{
    public class RemoveSeedlingsCommandValidator : AbstractValidator<RemoveSeedlingsCommand>
    {
        public RemoveSeedlingsCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cây giống không được để trống.");
        }
    }
}
