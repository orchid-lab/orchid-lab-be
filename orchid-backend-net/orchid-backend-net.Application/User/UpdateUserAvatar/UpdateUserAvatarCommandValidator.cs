using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateUserAvatar
{
    public class UpdateUserAvatarCommandValidator : AbstractValidator<UpdateUserAvatarCommand>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserAvatarCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .NotNull()
                .WithMessage("File name cannot be empty.");
            RuleFor(x => x.FileStream)
                .NotNull()
                .NotEmpty()
                .WithMessage("File cannot be empty.");
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("User ID cannot be empty.");
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) => await IsUserExist(id, cancellationToken))
                .WithMessage(x => $"User with id: {x.Id} can not be found");
        }
        private async Task<bool> IsUserExist(string id, CancellationToken cancellationToken)
            => await _userRepository.AnyAsync(x => x.ID.Equals(id) && x.Status, cancellationToken);
    }
}
