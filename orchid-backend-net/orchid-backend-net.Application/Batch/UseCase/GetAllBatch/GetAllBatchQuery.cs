using MediatR;
using orchid_backend_net.Application.Batch.Dto.Batch;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.UseCase.GetAllBatch
{
    public record GetAllBatchQuery(int PageNo, int PageSize, string? BatchNameSearchTerm, string? LabNameSearchTerm) : IRequest<PageResult<BatchDto>>;
    internal class GetAllBatchQueryHandler(IBatchesRepository batchesRepository) : IRequestHandler<GetAllBatchQuery, PageResult<BatchDto>>
    {
        public async Task<PageResult<BatchDto>> Handle(GetAllBatchQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Batches> queryOptions(IQueryable<Batches> query)
            {
                if (!string.IsNullOrWhiteSpace(request.BatchNameSearchTerm))
                    query = query.Where(b => b.BatchName.ToLower().Contains(request.BatchNameSearchTerm.ToLower()));
                if (!string.IsNullOrWhiteSpace(request.LabNameSearchTerm))
                    query = query.Where(b => b.LabRoom.Name.ToLower().Contains(request.LabNameSearchTerm.ToLower()));

                return query.OrderBy(x => x.Status);
            }

            var batch = await batchesRepository.FindAllProjectToAsync<BatchDto>(
                pageNo: request.PageNo,
                pageSize: request.PageSize,
                queryOptions: queryOptions,
                cancellationToken);

            return batch.ToAppPageResult();
        }
    }
}
