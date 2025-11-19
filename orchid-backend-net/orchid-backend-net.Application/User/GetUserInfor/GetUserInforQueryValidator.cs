using FluentValidation;

namespace orchid_backend_net.Application.User.GetUserInfor
{
    public class GetUserInforQueryValidator : AbstractValidator<GetUserInforQuery>
    {
        public GetUserInforQueryValidator()
        {
            RuleFor(x => x.ID)
                .NotNull()
                .NotEmpty()
                .WithMessage("Can not Empty UserID.");
        }
    }
}
