using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public CreateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            Configuration();
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        void Configuration()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name cannot be null or empty.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Email cannot be null or empty and must be a valid email address.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .MustAsync(async (email, cancellationToken) => !await IsEmailUnique(email, cancellationToken))
                .WithMessage("Email already exists.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (phoneNumber, cancellationToken) => !await IsPhoneNumberUnique(phoneNumber, cancellationToken))
                .WithMessage("Phone number cannot be null or empty and must be unique.");

            RuleFor(x => x.RoleID)
                .NotEmpty()
                .NotNull()
                .MustAsync((roleId, cancellationToken) => IsRoleValid(roleId, cancellationToken))
                .WithMessage("Role ID cannot be null or empty and must be exist.");
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()), cancellationToken);
        }

        private async Task<bool> IsPhoneNumberUnique(string phoneNumber, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(x => x.PhoneNumber.Equals(phoneNumber), cancellationToken);
        }

        private async Task<bool> IsRoleValid(int roleId, CancellationToken cancellationToken)
        {
            return await _roleRepository.AnyAsync(x => x.ID.Equals(roleId), cancellationToken);
        }
    }
}
