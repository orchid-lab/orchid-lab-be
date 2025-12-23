using MediatR;
using orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Authentication.Login
{
    public record LoginQuery(string Email, string Password) : IRequest<LoginDTO>, IQuery
    {
        public string Email { get; set; } = Email;
        public string Password { get; set; } = Password;
    }
    internal class LoginQueryHandler(IUserRepository _userRepository, ISender sender) : IRequestHandler<LoginQuery, LoginDTO>
    {
        public async Task<LoginDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(_ => _.Email == request.Email, cancellationToken) ?? throw new NotFoundException("Không tìm thấy người dùng.");
            if(user.DeletedDate != null)
            {
                throw new NotFoundException("Tài khoản đã bị vô hiệu hóa.");
            }
            var isTrue = _userRepository.VerifyPassword(request.Password, user.Password);
            if (!isTrue)
            {
                throw new IncorrectPasswordException("Sai mật khẩu.");
            }
            string Role = "";
            Role = user.RoleID switch
            {
                1 => "Admin",
                2 => "Researcher",
                3 => "Technician",
                _ => throw new NotImplementedException("Tài khoản này chưa có vai trò xác định."),
            };
            var refresh = await sender.Send(new RefreshTokenCommand(user.ID), cancellationToken);
            user.RefreshToken = refresh.Token;
            user.RefreshTokenExpiryTime = refresh.Expired;
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return LoginDTO.Create(user.ID, Role, refresh.Token, user.Name);
        }
    }
}
