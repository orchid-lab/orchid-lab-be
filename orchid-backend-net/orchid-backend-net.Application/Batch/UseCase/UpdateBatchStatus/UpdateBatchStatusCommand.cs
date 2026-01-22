using MediatR;
using orchid_backend_net.Application.Batch.Helper;
using orchid_backend_net.Application.Batch.Policy;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Batch.UseCase.UpdateBatchStatus
{
    public record UpdateBatchStatusCommand(int Id, string Status) : IRequest<string>;
    internal class UpdateBatchStatusCommandHandler(
        IBatchesRepository batchesRepository,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateBatchStatusCommand, string>
    {
        public async Task<string> Handle(UpdateBatchStatusCommand request, CancellationToken cancellationToken)
        {
            var batch = await batchesRepository.FindAsync(x => x.ID == request.Id, cancellationToken) ?? throw new ArgumentException("Batch not found");
            // Validate status change
            var newStatus = BatchPolicy.ValidateBatchStatusChange(request.Status);

            //event trigger
            BatchStatusActionDispatcher.Dispatch(batch, newStatus, currentUserService.UserId!);

            batchesRepository.Update(batch);
            return await batchesRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? batch.ID.ToString()
                : "Cập nhật thất bại";
        }
    }
}
