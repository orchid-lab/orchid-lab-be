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


    internal class RefreshTokenQueryHandler(IUserRepository userRepository, ICacheService cacheService,
        ICurrentUserService currentUserService, ISender sender) : IRequestHandler<RefreshTokenQuery, LoginDTO>
    {
        public async Task<LoginDTO> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {

            var refreshTokenKey = request.RefreshToken.Trim().ToLowerInvariant();
            var redisKey = $"auth:refresh_token:{refreshTokenKey}";
            var userId = await cacheService.GetAsync(redisKey);


            //Check if the refresh token exists in Redis
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            //Check if the user exists in the database
            var user = await userRepository.FindAsync(x => x.ID.Equals(userId)
                        && x.Status == true
                        && x.RefreshTokenExpiryTime >= DateTime.UtcNow, cancellationToken);

            //Fallback if the user is not found
            user ??= await userRepository.FindAsync(x => x.RefreshToken!.Equals(request.RefreshToken.Trim())
                        && x.Status == true
                        && x.RefreshTokenExpiryTime >= DateTime.UtcNow, cancellationToken);

            if(user is null)
                throw new UnauthorizedAccessException("User not found or token expired");

            //Token rotation
            await cacheService.RemoveAsync(redisKey);

            string role = "";
            role = user.RoleID switch
            {
                0 => "Account does not have a role",
                1 => "Admin",
                2 => "Researcher",
                3 => "Technician",
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
