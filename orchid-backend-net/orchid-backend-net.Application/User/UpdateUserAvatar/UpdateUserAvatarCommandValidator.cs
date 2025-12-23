using FluentValidation;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateUserAvatar
{
    public class UpdateUserAvatarCommandValidator : AbstractValidator<UpdateUserAvatarCommand>
    {
        public UpdateUserAvatarCommandValidator(IUserRepository userRepository)
        {
            Configure();
        }

        private void Configure()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Tên tệp tin phải hợp lệ.");
            RuleFor(x => x.FileStream)
                .NotNull()
                .NotEmpty()
                .WithMessage("Tệp tin không được để trống.");
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("Người dùng phải tồn tại.");
        }
    }
}
