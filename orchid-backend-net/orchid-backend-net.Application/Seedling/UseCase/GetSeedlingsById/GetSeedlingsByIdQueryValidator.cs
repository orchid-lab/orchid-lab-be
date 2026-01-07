using FluentValidation;

namespace orchid_backend_net.Application.Seedling.UseCase.GetSeedlingsById
{
    public class GetSeedlingsByIdQueryValidator : AbstractValidator<GetSeedlingsByIdQuery>
    {
        public GetSeedlingsByIdQueryValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id không được để trống.");
        }
    }
}
