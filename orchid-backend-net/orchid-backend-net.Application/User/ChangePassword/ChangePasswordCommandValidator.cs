using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.User.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            Configuration();
        }
        void Configuration()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .NotNull()
                .WithMessage("User ID không được để trống.");
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .NotNull()
                .WithMessage("Mật khẩu hiện tại không được để trống.");
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .NotNull()
                .WithMessage("Mật khẩu mới không được để trống.");
        }
    }
}
