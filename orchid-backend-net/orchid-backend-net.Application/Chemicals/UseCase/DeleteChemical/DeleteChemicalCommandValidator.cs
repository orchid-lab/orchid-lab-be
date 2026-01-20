using FluentValidation;

namespace orchid_backend_net.Application.Chemicals.UseCase.DeleteChemical
{
    public class DeleteChemicalCommandValidator : AbstractValidator<DeleteChemicalCommand>
    {
        public DeleteChemicalCommandValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Id phải lớn hơn 0");
        }
    }
}
