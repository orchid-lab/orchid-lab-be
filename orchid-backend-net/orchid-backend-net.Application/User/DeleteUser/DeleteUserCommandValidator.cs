using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id can not be null or empty.");

            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) => await IsUserExist(id, cancellationToken))
                .WithMessage(x => $"Can not found user with id: {x.Id}");
        }

        private async Task<bool> IsUserExist(string id, CancellationToken cancellationToken)
            => await _userRepository.AnyAsync(x => x.ID.Equals(id) && x.Status, cancellationToken);
    }
}
