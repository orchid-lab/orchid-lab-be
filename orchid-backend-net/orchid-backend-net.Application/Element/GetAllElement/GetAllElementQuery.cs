using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Element.GetAllElement
{
    public class GetAllElementQuery : IRequest<PageResult<ElementDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetAllElementQuery(int pagenumber, int pagesize)
        {
            this.PageNumber = pagenumber;
            this.PageSize = pagesize;
        }
        public GetAllElementQuery() { }
    }

    internal class GetAllElementQueryHandler(IElementRepositoty elementRepositoty, IMapper mapper) : IRequestHandler<GetAllElementQuery, PageResult<ElementDTO>>
    {
        public async Task<PageResult<ElementDTO>> Handle(GetAllElementQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await elementRepositoty.FindAllAsync(x => x.Status == true, request.PageNumber, request.PageSize, cancellationToken);
                if (list.Count() == 0)
                    throw new NotFoundException("there're no element in the system.");
                return PageResult<ElementDTO>.Create(totalCount: list.TotalCount,
                    pageCount: list.PageCount,
                    pageSize: list.PageSize,
                    pageNumber: list.PageNo,
                    data: list.MapToElementDTOList(mapper)
                    );
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
