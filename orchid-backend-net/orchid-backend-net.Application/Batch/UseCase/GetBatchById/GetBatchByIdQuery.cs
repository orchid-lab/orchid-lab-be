using MediatR;
using orchid_backend_net.Application.Batch.Dto.Batch;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.UseCase.GetBatchById
{
    public record GetBatchByIdQuery(int Id) : IRequest<BatchDto>;
    internal class GetBatchByIdQueryHandler(IBatchesRepository batchesRepository) : IRequestHandler<GetBatchByIdQuery, BatchDto>
    {
        public async Task<BatchDto> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
        {
            var batch = await batchesRepository.FindProjectToAsync<BatchDto>(
                queryOptions: q => q.Where(b => b.ID == request.Id),
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy batch này");
            return batch;
        }
    }
}
