using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.UseCase.CreateBatch
{
    public record CreateBatchCommand(
        int LabRoomId, 
        string BatchName, 
        decimal BatchSizeWidth, 
        decimal BatchSizeHeight, 
        string WidthUnit, 
        string HeightUnit) : IRequest<string>;
    internal class CreateBatchCommandHandler(IBatchesRepository batchesRepository) : IRequestHandler<CreateBatchCommand, string>
    {
        public async Task<string> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await batchesRepository.FindAsync(b => b.BatchName == request.BatchName, cancellationToken);
            if (batch is not null)
            {
                throw new InvalidOperationException($"Batch with name {request.BatchName} already exists.");
            }
            var newBatch = new Domain.Entities.Batches
            {
                LabRoomId = request.LabRoomId,
                BatchName = request.BatchName,
                BatchSizeWidth = request.BatchSizeWidth,
                BatchSizeHeight = request.BatchSizeHeight,
                WidthUnit = request.WidthUnit,
                HeightUnit = request.HeightUnit,
                IsBatching = false
            };
            batchesRepository.Add(newBatch);
            return await batchesRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? newBatch.ID.ToString()
                : throw new Exception("Failed to create batch.");

        }
    }
}
