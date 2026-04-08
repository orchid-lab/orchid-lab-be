using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.DeleteExperimentLog
{
    public record DeleteExperimentLogCommand(string Id, string? Reason, string Conclusion, string Issue, string Recommendation) : IRequest<string>;
    internal class DeleteExperimentLogCommandHandler(
        IExperimentLogRepository experimentLogRepository,
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteExperimentLogCommand, string>
    {
        public async Task<string> Handle(DeleteExperimentLogCommand request, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.FindAsync(el => el.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Experiment log not found");
            experimentLog.DestroyBecauseAllSamplesInfected(request.Reason, request.Conclusion, request.Issue, request.Recommendation);
            experimentLog.UpdatedBy = currentUserService.UserId;
            experimentLog.UpdatedDate = DateTime.UtcNow;

            var allTaskInExperiment = await taskRepository.FindAllAsync(t =>
            t.TaskAssignment.TargetId == experimentLog.ID &&
            t.TaskAssignment.TargetType == TaskTargetType.ExperimentLog, cancellationToken); 

            if(allTaskInExperiment != null)
            {
                foreach(var task in allTaskInExperiment)
                {
                    task.TaskCancelled(request.Reason);
                    task.UpdatedBy = currentUserService.UserId;
                    task.UpdatedDate = DateTime.UtcNow;
                    taskRepository.Update(task);
                }
            }

            experimentLogRepository.Update(experimentLog);
            return await unitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? experimentLog.ID.ToString()
                : "Hủy thí nghiệm thất bại";
        }
    }
}
