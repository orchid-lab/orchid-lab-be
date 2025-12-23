using AutoMapper;
using orchid_backend_net.Application.Common.Mappings;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.User
{
    public class UserDto : IMapFrom<Users>
    {
        public required string ID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AvatarUrl { get; set; }

        public static UserDto Create(UserDtoParameter dtoParameter)
        {
            return new UserDto
            {
                ID = dtoParameter.ID,
                Name = dtoParameter.Name,
                Email = dtoParameter.Email,
                Password = dtoParameter.Password,
                PhoneNumber = dtoParameter.PhoneNumber,
                Role = dtoParameter.Role,
                CreatedDate = dtoParameter.CreatedDate,
                CreatedBy = dtoParameter.CreatedBy,
                DeletedDate = dtoParameter.DeletedDate,
                DeletedBy = dtoParameter.DeletedBy,
                UpdatedDate = dtoParameter.UpdatedDate,
                UpdatedBy = dtoParameter.UpdatedBy,
                AvatarUrl = dtoParameter.AvatarUrl
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Users, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role!.Name));
        }

        public class UserDtoParameter
        {
            public required string ID { get; set; }
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string PhoneNumber { get; set; }
            public string Role { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string? CreatedBy { get; set; }
            public string? AvatarUrl { get; set; }
            public DateTime? DeletedDate { get; internal set; }
            public string? DeletedBy { get; internal set; }
            public string? UpdatedBy { get; internal set; }
            public DateTime? UpdatedDate { get; internal set; }
        }
    }
}
