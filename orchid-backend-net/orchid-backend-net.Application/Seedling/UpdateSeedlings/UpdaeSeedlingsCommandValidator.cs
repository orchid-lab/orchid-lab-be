using FluentValidation;

namespace orchid_backend_net.Application.Seedling.UpdateSeedlings
{
    public class UpdaeSeedlingsCommandValidator : AbstractValidator<UpdateSeedlingsCommand>
    {
        public UpdaeSeedlingsCommandValidator() { }
        public void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id không được để trống.");
        }
    }
}
