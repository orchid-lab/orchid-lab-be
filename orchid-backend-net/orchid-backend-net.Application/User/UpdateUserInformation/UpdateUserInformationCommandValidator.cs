using FluentValidation;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateUser
{
    public class UpdateUserInformationCommandValidator : AbstractValidator<UpdateUserInformationCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        public UpdateUserInformationCommandValidator(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;   
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(u => !string.IsNullOrEmpty(u.Email))
                .WithMessage("Email không hợp lệ.");

            RuleFor(x => x)
                .MustAsync(async (request, cancellationToken) => await BeUniqueEmail(request.Email,cancellationToken))
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

            RuleFor(x => x)
                .MustAsync(async (request, cancellationToken) => await BeUniquePhoneNumber(request.PhoneNumber, cancellationToken))
                .When(u => !string.IsNullOrEmpty(u.PhoneNumber))
                .WithMessage("Số điện thoại đã tồn tại.");
        }

        private async Task<bool> BeUniqueEmail( string? email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            var existedUser = await _userRepository
                .FindAsync(u => u.Email == email, cancellationToken);

            // Không ai dùng email này
            if (existedUser == null)
                return true;

            // Email thuộc chính user đang update
            return existedUser.ID == _currentUserService.UserId;
        }

        private async Task<bool> BeUniquePhoneNumber(string? phoneNumber, CancellationToken cancellationToken)
        {
            // Không có phone => không cần check unique
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true;

            // Tìm user đang sở hữu số điện thoại này (nếu có)
            var existedUser = await _userRepository
                .FindAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

            // Không ai dùng số này => OK
            if (existedUser == null)
                return true;

            // Số này thuộc chính user đang update => OK
            return existedUser.ID == _currentUserService.UserId;
        }
    }
}
