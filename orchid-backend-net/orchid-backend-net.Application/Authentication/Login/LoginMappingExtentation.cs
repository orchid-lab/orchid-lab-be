using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Authentication.Login
{
    public static class LoginDTOMappingExstensions
    {
        public static LoginDTO MapToLoginDTO(this Users user, IMapper mapper)
        => mapper.Map<LoginDTO>(user);
    }
}
