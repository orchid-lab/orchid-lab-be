using FluentValidation;

namespace orchid_backend_net.Application.User.GetUserId
{
    public class GetUserIdValidator : AbstractValidator<GetUserIdQuery>
    {
        public GetUserIdValidator()
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id không được để trống.");
        }
    }
}
