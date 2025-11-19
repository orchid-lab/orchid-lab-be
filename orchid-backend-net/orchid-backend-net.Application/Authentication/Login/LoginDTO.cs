using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Authentication.Login
{
    public class LoginDTO : IMapFrom<Users>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Users, LoginDTO>();

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string RoleID { get; set; }
        public required string RefreshToken { get; set; }

        public static LoginDTO Create(string userID, string role, string refreshToken, string name)
        {
            return new LoginDTO
            {
                Id = userID,
                RoleID = role,
                RefreshToken = refreshToken,
                Name = name
            };
        }

    }
}
