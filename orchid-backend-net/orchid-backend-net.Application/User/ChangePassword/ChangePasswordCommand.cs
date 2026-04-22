using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.User.ChangePassword
{
    public class ChangePasswordCommand : IRequest<string>
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public ChangePasswordCommand(string userId, string currentPassword, string newPassword)
        {
            UserId = userId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
        public ChangePasswordCommand()
        {
        }
    }
    internal class ChangePasswordCommandHandler(IUserRepository userRepository) : 
        IRequestHandler<ChangePasswordCommand, string>
    {
        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(x => x.ID.ToString() == request.UserId);
            if (user == null)
                throw new Exception("User not found");
            var isTrue = userRepository.VerifyPassword(request.CurrentPassword , user.Password);
            if (!isTrue)
                throw new Exception("Current password is incorrect");
            user.Password = userRepository.HashPassword(request.NewPassword);
            userRepository.Update(user);
            return (await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken)) > 0 ?
                $"Password changed successfully user account : {user.ID}." :
                "Password failed change.";
        }
    }
}
