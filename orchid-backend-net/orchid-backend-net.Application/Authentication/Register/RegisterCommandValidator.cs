using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Authentication.Register
{
    internal class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public RegisterCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
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
                .WithMessage("Tên không được để trống.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Email không được để trống và phải là một email hợp lệ.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .MustAsync(async (email, cancellationToken) => !await IsEmailUnique(email, cancellationToken))
                .WithMessage("Email đã tồn tại.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .MustAsync(async (phoneNumber, cancellationToken) => !await IsPhoneNumberUnique(phoneNumber, cancellationToken))
                .WithMessage("Số điện thoại không được để trống và phải là số điện thoại hợp lệ.");

            RuleFor(x => x.RoleID)
                .NotEmpty()
                .NotNull()
                .MustAsync((roleId, cancellationToken) => IsRoleValid(roleId, cancellationToken))
                .WithMessage("Công việc của tài khoản này phải hợp lệ.");
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
