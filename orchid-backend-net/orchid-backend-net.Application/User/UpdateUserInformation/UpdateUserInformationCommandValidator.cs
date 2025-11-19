using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateUser
{
    public class UpdateUserInformationCommandValidator : AbstractValidator<UpdateUserInformationCommand>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserInformationCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("User id can not be null or empty.");
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) => await IsUserExist(id, cancellationToken))
                .WithMessage(x => $"User with id: {x.Id} can not be found.");
        }
        private async Task<bool> IsUserExist(string id, CancellationToken cancellationToken)
            => await _userRepository.AnyAsync(x => x.ID.Equals(id), cancellationToken);
    }
}
