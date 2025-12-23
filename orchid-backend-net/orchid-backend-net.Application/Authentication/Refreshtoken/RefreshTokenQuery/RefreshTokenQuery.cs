using MediatR;
using orchid_backend_net.Application.Authentication.Login;
using orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Authentication.Refreshtoken.RefreshTokenQuery
{
    public class RefreshTokenQuery(string refreshToken) : IRequest<LoginDTO>
    {
        public string RefreshToken { get; set; } = refreshToken;
    }


    internal class RefreshTokenQueryHandler(IUserRepository userRepository, ICacheService cacheService, ISender sender) : IRequestHandler<RefreshTokenQuery, LoginDTO>
    {
        public async Task<LoginDTO> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {

            var refreshTokenKey = request.RefreshToken.Trim().ToLowerInvariant();
            var redisKey = $"auth:refresh_token:{refreshTokenKey}";
            var userId = await cacheService.GetAsync(redisKey);


            //Check if the refresh token exists in Redis
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Refresh Token không hợp lệ.");
            }

            //Check if the user exists in the database
            var user = await userRepository.FindAsync(x => x.ID.Equals(userId)
                        && x.DeletedDate == null
                        && x.RefreshTokenExpiryTime >= DateTime.UtcNow, cancellationToken);

            if (user is null)
                throw new UnauthorizedAccessException("Không tìm thấy người dùng hoặc token không hợp lệ.");

            //Token rotation
            var isRemoveSuccess = await cacheService.RemoveAsync(redisKey);

            if (!isRemoveSuccess)
                throw new InvalidOperationException("Có lỗi xảy ra, vui lòng thử lại sau.");

            string role = "";
            role = user.RoleID switch
            {
                1 => "Admin",
                2 => "Researcher",
                3 => "Technician",
                _ => throw new NotImplementedException("Tài khoản này chưa có vai trò xác định."),
            };

            //Generate new token
            var refresh = await sender.Send(new RefreshTokenCommand(user.ID), cancellationToken);
            user.RefreshToken = refresh.Token;
            user.RefreshTokenExpiryTime = refresh.Expired;
            await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return LoginDTO.Create(user.ID, role, user.RefreshToken, user.Name);
        }
    }
}
