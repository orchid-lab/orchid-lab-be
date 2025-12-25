using MediatR;
using orchid_backend_net.Application.Common.Enum;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities.Base;
using System.Security.Cryptography;

namespace orchid_backend_net.Application.Authentication.Refreshtoken.GenerateRefreshToken
{
    public class RefreshTokenCommand(string userID) : IRequest<RefreshToken>, ICommand
    {
        public string UserID { get; set; } = userID;
    }
    internal class RefreshTokenCommandHandler(ICacheService cacheService) : IRequestHandler<RefreshTokenCommand, RefreshToken>
    {
        public async Task<RefreshToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            string token = GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = token,
                Expired = TimeZoneEnum.VietnamTimeZone
            };


            var expiryDateInRedis = refreshToken.Expired - DateTime.UtcNow;
            //Store the refresh token in Redis with a key based on the token value
            //best practice: https://cheatsheetseries.owasp.org/cheatsheets/JSON_Web_Token_for_Java_Cheat_Sheet.html#token-refresh
            var refreshTokenKey = refreshToken.Token.Trim().ToLowerInvariant();
            var cacheKey = $"auth:refresh_token:{refreshTokenKey}";
            await cacheService.SetAsync(cacheKey, request.UserID, expiryDateInRedis);

            return refreshToken;
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
