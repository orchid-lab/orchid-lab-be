using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetAllMethod
{
    public class GetAllMethodQuery : IRequest<PageResult<MethodDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
    }

    internal class GetAllMethodQueryHandler(IMethodRepository methodRepository) : IRequestHandler<GetAllMethodQuery, PageResult<MethodDto>>
    {
        public async Task<PageResult<MethodDto>> Handle(GetAllMethodQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Methods> queryOptions(IQueryable<Methods> query)
            {
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                    query = query.Where(m => m.Name.Contains(request.SearchTerm));
                return query;
            }

            var methods = await methodRepository.FindAllProjectToAsync<MethodDto>(
                pageNo: request.PageNumber,
                pageSize: request.PageSize,
                queryOptions,
                cancellationToken);
            return methods.ToAppPageResult();
        }
    }
}
