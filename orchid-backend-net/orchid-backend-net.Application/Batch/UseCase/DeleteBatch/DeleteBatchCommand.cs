using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.UseCase.DeleteBatch
{
    public record DeleteBatchCommand(int Id) : IRequest<string>;
    internal class DeleteBatchCommandHandler(IBatchesRepository batchesRepository) : IRequestHandler<DeleteBatchCommand, string>
    {
        public async Task<string> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await batchesRepository.FindAsync(b => b.ID == request.Id, cancellationToken);
            if (batch is null)
            {
                throw new NotFoundException($"không tìm thấy batch này.");
            }
            if (batch.IsBatching)
                throw new InvalidOperationException("Không thể xóa batch đang trong quá trình thực hiện.");
            batchesRepository.Remove(batch);
            return await batchesRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? request.Id.ToString()
                : "Xóa batch thất bại";
        }
    }
}
