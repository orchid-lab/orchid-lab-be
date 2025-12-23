using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.GetUserId
{
    public class GetUserIdQuery : IRequest<UserDto>
    {
        public string Id { get; set; }
        public GetUserIdQuery(string id)
        {
            Id = id;
        }
    }

    internal class GetUserIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindProjectToAsync<UserDto>(queryOptions: query => query.Where(u => u.ID.Equals(request.Id)), cancellationToken);
            return user ?? throw new NotFoundException("Không tìm thấy người dùng.");
        }
    }
}
