using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TissueCultureBatch.GetAllTissueCultureBatch
{
    public class GetAllTissueCultureBatchQuery(int pageNumber, int pageSize) : IRequest<PageResult<TissueCultureBatchDTO>>
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;

    }

    internal class GetAllTissueCultureBatchQueryHandler(ITissueCultureBatchRepository repository) : IRequestHandler<GetAllTissueCultureBatchQuery, PageResult<TissueCultureBatchDTO>>
    {
        public async Task<PageResult<TissueCultureBatchDTO>> Handle(GetAllTissueCultureBatchQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tissueCultureBatches = await repository.FindAllProjectToAsync<TissueCultureBatchDTO>(
                    pageNo: request.PageNumber,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);
                return tissueCultureBatches.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
    }
}
