using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.User
{
    public static class UserMappingExtentations
    {
        public static UserDTO MapToUserDTO(this Users user, IMapper mapper)
            => mapper.Map<UserDTO>(user);
        public static List<UserDTO> MapToUserDTOList(this IEnumerable<orchid_backend_net.Domain.Entities.Users> userList, IMapper mapper)
            => userList.Select(x => x.MapToUserDTO(mapper)).ToList();
    }
}
