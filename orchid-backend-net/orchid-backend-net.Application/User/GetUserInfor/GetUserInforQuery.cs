using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.GetUserInfor
{
    public class GetUserInforQuery : IRequest<UserDTO>, IQuery
    {
        public string ID { get; set; }
        public GetUserInforQuery(string id)
        {
            this.ID = id;
        }
        public GetUserInforQuery() { }

    }

    public class GetUserInforQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserInforQuery, UserDTO>
    {
        public async Task<UserDTO> Handle(GetUserInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                return user.MapToUserDTO(mapper);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
