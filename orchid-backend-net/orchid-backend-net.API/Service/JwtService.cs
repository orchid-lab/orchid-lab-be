using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using orchid_backend_net.Application.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace orchid_backend_net.API.Service
{
    /// <summary>
    /// Jwt Service for create JWT Token
    /// </summary>
    public class JwtService
    {
        /// <summary>
        /// jwt token class
        /// </summary>
        public class Token
        {
            /// <summary>
            /// AccessToken JWT using for authorization
            /// </summary>
            public required string AccessToken { get; set; }
            /// <summary>
            /// RefreshToken JWT using for get new AccessToken
            /// </summary>
            public required string RefreshToken { get; set; }
            /// <summary>
            /// User information
            /// </summary>
            public UserDto? UserDTO { get; set; } = null;
        }

        /// <summary>
        /// Create JWT Token method
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="roles"></param>
        /// <param name="refreshToken"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Token CreateToken(string ID, string roles, string refreshToken, string name)
        {
            var claims = new List<Claim>
            {

                new(JwtRegisteredClaimNames.Sub, ID.ToString()),
                new(ClaimTypes.Role, roles.ToString()),
                new(JwtClaimTypes.Name, name),
                new("RoleName",roles.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OrchidLabManagementSystemsDotNetApi"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://net-api.tissuex.me/",
                audience: "api",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);
            var re = new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
            return re;
        }

        /// <summary>
        /// Get claims from expired access token for refresh
        /// </summary>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = "api",
                ValidateIssuer = true,
                ValidIssuer = "https://net-api.tissuex.me/",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("OrchidLabManagementSystemsDotNetApi")),
                ValidateLifetime = false, // Allow expired tokens for refresh
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
