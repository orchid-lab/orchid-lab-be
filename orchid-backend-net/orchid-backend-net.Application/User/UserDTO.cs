using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.User
{
    public class UserDTO : IMapFrom<Users>
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleID { get; set; }
        public DateTime Create_at { get; set; }
        public string Create_by { get; set; }
        public string? AvatarUrl { get; set; }

        public static UserDTO Create(string id, string name, string email, string phoneNumber, string? avtUrl, string create_by, DateTime create_at)
        {
            return new UserDTO
            {
                ID = id,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                AvatarUrl = avtUrl,
                Create_at = create_at,
                Create_by = create_by,
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Users, UserDTO>();
        }
    }
}
