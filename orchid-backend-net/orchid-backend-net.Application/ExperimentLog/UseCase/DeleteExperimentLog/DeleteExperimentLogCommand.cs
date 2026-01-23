using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.DeleteExperimentLog
{
    public record DeleteExperimentLogCommand(string Id, string? Reason) : IRequest<string>;
    internal class DeleteExperimentLogCommandHandler(IExperimentLogRepository experimentLogRepository, ICurrentUserService currentUserService) : IRequestHandler<DeleteExperimentLogCommand, string>
    {
        public async Task<string> Handle(DeleteExperimentLogCommand request, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.FindAsync(el => el.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Experiment log not found");
            experimentLog.DestroyBecauseAllSamplesInfected(request.Reason);
            experimentLog.UpdatedBy = currentUserService.UserId;
            experimentLog.UpdatedDate = DateTime.UtcNow;
            experimentLogRepository.Update(experimentLog);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? experimentLog.ID.ToString()
                : "Hủy thí nghiệm thất bại";
        }
    }
}
