using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ExperimentLog.Helper;
using orchid_backend_net.Application.ExperimentLog.Policy;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogStatus
{
    public record UpdateExperimentLogStatusCommand(
        string Id,
        string Status,
        int? BatchId) : IRequest<string>;

    internal class UpdateExperimentLogStatusCommandHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        IBatchesRepository batchesRepository,
        ICurrentUserService currentUserService
        ) : IRequestHandler<UpdateExperimentLogStatusCommand, string>
    {
        public async Task<string> Handle(UpdateExperimentLogStatusCommand request, CancellationToken cancellationToken)
        {
            //validate status
            var nextStatus = ExperimentLogPolicy.ValidateStatusChange(request.Status);

            //get experiment logs to change status  
            var experimentLogs = await experimentLogRepository.FindAsync(
                el => el.ID == request.Id
                && el.CreatedBy == currentUserService.UserId,
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy thí nghiệm này");

            //get method and method stages to find next stage
            var method = await methodRepository.FindAsync(m => m.ID == experimentLogs.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy phương pháp liên quan đến thí nghiệm này");
            var methodStages = method.MethodStages;
            var nextStage = methodStages.FirstOrDefault(ms => ms.Order == experimentLogs.CurrentStageOrder + 1)
                ?? throw new InvalidOperationException("Thí nghiệm này đã ở giai đoạn cuối của phương pháp thí nghiệm");

            //get batch if provided
            if (request.BatchId is not null && request.BatchId > 0)
            {
                var batch = await batchesRepository.FindAsync(b => b.ID == request.BatchId, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy batch này");
                var oldBatch = experimentLogs.Batch;
                oldBatch.FinishBatching(currentUserService.UserId!);
                experimentLogs.BatchId = batch.ID;
                batch.StartBatching();
            }

            //update experiment log status and stage
            ExperimentLogStatusDispatcher.Dispatch(
                experimentLogs,
                nextStatus,
                nextStage,
                currentUserService.UserId!);

            //update and save changes
            experimentLogRepository.Update(experimentLogs);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? 
                experimentLogs.ID.ToString() 
                : "Cập nhật thất bại";
        }
    }
}
