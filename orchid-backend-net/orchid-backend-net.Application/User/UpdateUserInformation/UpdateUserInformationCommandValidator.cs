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
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(u => !string.IsNullOrEmpty(u.Email))
                .WithMessage("Email không hợp lệ.");

            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellationToken) => await BeUniqueEmail(email,cancellationToken))
                .When(u => !string.IsNullOrEmpty(u.Email))
                .WithMessage("Email đã tồn tại.");

            RuleFor(x => x.PhoneNumber)
                //when true => pass
                //when false => throw message
                .Matches(@"^\+?[0-9]\d{1,14}$")
                //when false => pass case
                //when true => run case check matches
                .When(u => !string.IsNullOrEmpty(u.PhoneNumber))
                .WithMessage("Số điện thoại không hợp lệ.");

            RuleFor(x => x.PhoneNumber)
                .MustAsync(async (phoneNumber, cancellationToken) => await BeUniquePhoneNumber(phoneNumber, cancellationToken))
                .When(u => !string.IsNullOrEmpty(u.PhoneNumber))
                .WithMessage("Số điện thoại đã tồn tại.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(u => u.Email == email, cancellationToken);
            return user == null;
        }

        private async Task<bool> BeUniquePhoneNumber(string phoneNumber, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
            return user == null;
        }
    }
}
