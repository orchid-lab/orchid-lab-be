using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.UseCase.UpdateBatch
{
    public record UpdateBatchCommand(
        int Id,
        int? LabRoomId,
        string? BatchName,
        decimal? BatchSizeWidth,
        decimal? BatchSizeHeight,
        string? WidthUnit,
        string? HeightUnit) : IRequest<string>;
    internal class UpdateBatchCommandHandler(IBatchesRepository batchesRepository) : IRequestHandler<UpdateBatchCommand, string>
    {
        public async Task<string> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await batchesRepository.FindAsync(b => b.ID == request.Id, cancellationToken);
            if (batch is null)
            {
                throw new NotFoundException($"không tìm thấy batch này.");
            }
            if(batch.IsBatching)
            {
                return "Không thể cập nhật batch đang trong quá trình thực hiện.";
            }
            batch.LabRoomId = request.LabRoomId ?? batch.LabRoomId;
            batch.BatchName = request.BatchName ?? batch.BatchName;
            batch.BatchSizeWidth = request.BatchSizeWidth ?? batch.BatchSizeWidth;
            batch.BatchSizeHeight = request.BatchSizeHeight ?? batch.BatchSizeHeight;
            batch.WidthUnit = request.WidthUnit ?? batch.WidthUnit;
            batch.HeightUnit = request.HeightUnit ?? batch.HeightUnit;
            batchesRepository.Update(batch);
            return await batchesRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? batch.ID.ToString()
                : "Cập nhật batch thất bại";
        }
    }
}
