using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.GetAllMethod
{
    public class GetAllMethodQuery : IRequest<PageResult<MethodDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Filter { get; set; }
        public GetAllMethodQuery() { }
        public GetAllMethodQuery(int pageNumber, int pageSize, string? filter)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            Filter = filter;
        }
    }

    internal class GetAllMethodQueryHandler : IRequestHandler<GetAllMethodQuery, PageResult<MethodDTO>>
    {
        private readonly IMethodRepository _methodRepository;
        public GetAllMethodQueryHandler(IMethodRepository methodRepository)
        {
            _methodRepository = methodRepository;
        }
        public async Task<PageResult<MethodDTO>> Handle(GetAllMethodQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Methods> queryOptions(IQueryable<Methods> query)
                {
                    query = query.Where(x => x.Status);
                    if (!string.IsNullOrEmpty(request.Filter))
                    {
                        query = query.Where(x => x.Type.ToLower().Contains(request.Filter.ToLower()) && x.Status);
                    }
                    return query;
                }

                var methods = await this._methodRepository.FindAllProjectToAsync<MethodDTO>(
                    request.PageNumber,
                    request.PageSize,
                    queryOptions: queryOptions,
                    cancellationToken);


                var list = await this._methodRepository.FindAllAsync(x => x.Status == true, request.PageNumber, request.PageSize, cancellationToken);
                //order by step in method's stages
                foreach (var item in list)
                {
                    item.Stages = [.. item.Stages.OrderBy(x => x.Step)];
                }
                return methods.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
