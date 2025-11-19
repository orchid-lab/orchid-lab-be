using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.GetAllUser
{
    public class GetAllUserQuery : IRequest<PageResult<UserDTO>>, IQuery
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

    internal class GetAllUserQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetAllUserQuery, PageResult<UserDTO>>
    {

        public async Task<PageResult<UserDTO>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var list = await userRepository.FindAllAsync(request.PageNumber, request.PageSize, cancellationToken); ;
            if (list.Count() == 0)
                throw new NotFoundException("there're no user in the system.");
            return PageResult<UserDTO>.Create(totalCount: list.TotalCount,
                pageCount: list.PageCount,
                pageSize: list.PageSize,
                pageNumber: list.PageNo,
                data: list.MapToUserDTOList(mapper)
                );
        }
    }
}
