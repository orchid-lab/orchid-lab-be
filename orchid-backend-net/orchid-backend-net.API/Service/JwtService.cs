using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using orchid_backend_net.Application.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace orchid_backend_net.API.Service
{
    public class JwtService
    {
        public class Token
        {
            public required string AccessToken { get; set; }
            public required string RefreshToken { get; set; }
            public UserDTO? UserDTO { get; set; } = null;
        }
        public Token CreateToken(string ID, string roles, string refreshToken, string name)
        {
            var claims = new List<Claim>
            {

                new(JwtRegisteredClaimNames.Sub, ID.ToString()),
                new(ClaimTypes.Role, roles.ToString()),
                new(JwtClaimTypes.Name, name),
                new("RoleName",roles.ToString())
            };


            //tf do u means ?
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OrchidLabManagementSystemsDotNetApi"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://net-api.orchid-lab.systems/",
                audience: "api",
                claims: claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: creds);
            var re = new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
            return re;
        }
    }
}
