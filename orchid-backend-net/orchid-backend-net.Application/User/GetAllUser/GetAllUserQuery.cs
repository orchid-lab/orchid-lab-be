using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.GetAllUser
{
    public class GetAllUserQuery : IRequest<PageResult<UserDto>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAllUserQuery(int pagenumber, int pagesize)
        {
            this.PageNumber = pagenumber;
            this.PageSize = pagesize;
        }
        public GetAllUserQuery() { }
    }

    internal class GetAllUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUserQuery, PageResult<UserDto>>
    {

        public async Task<PageResult<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var list = await userRepository.FindAllProjectToAsync<UserDto>(
                pageNo: request.PageNumber,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);
            return list.ToAppPageResult();
        }
    }
}
