using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ExperimentLog.Helper;
using orchid_backend_net.Application.ExperimentLog.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.UpdateExperimentLogStatus
{
    public record UpdateExperimentLogStatusCommand(
        string Id,
        string Status,
        int? BatchId,
        string? Reason) : IRequest<string>;

    internal class UpdateExperimentLogStatusCommandHandler(
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository,
        IBatchesRepository batchesRepository,
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService
        ) : IRequestHandler<UpdateExperimentLogStatusCommand, string>
    {
        public async Task<string> Handle(UpdateExperimentLogStatusCommand request, CancellationToken cancellationToken)
        {
            //validate status
            var nextStatus = ExperimentLogPolicy.ValidateStatusChange(request.Status);

            //get experiment logs to change status  
            var experimentLogs = await experimentLogRepository.FindAsync(
                el => el.ID == request.Id,
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

            //validate if technician want to move status to completed but there are still incomplete subtask
            if(nextStatus == Domain.Common.Enum.ExperimentLogStatus.WaitingForChangeStage) 
                await ExperimentLogPolicy.ValidateForChangeStage(experimentLogs.ID, taskRepository, cancellationToken);

            //update experiment log status and stage
            ExperimentLogStatusDispatcher.Dispatch(
                experimentLogs,
                nextStatus,
                nextStage,
                request.Reason);
            experimentLogs.UpdatedBy = currentUserService.UserId!;
            experimentLogs.UpdatedDate = DateTime.UtcNow;

            //validate if experiment log task has any incomplete subtask before moving to completed status

            //update and save changes
            experimentLogRepository.Update(experimentLogs);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                experimentLogs.ID.ToString()
                : "Cập nhật thất bại";
        }
    }
}
