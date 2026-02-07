using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.CancelExperimentLog
{
    public record CancelExperimentLogCommand(string Id, string? Reason) : IRequest<string>;
    internal class CancelExperimentLogCommandHandler(IExperimentLogRepository experimentLogRepository, ICurrentUserService currentUserService) : IRequestHandler<CancelExperimentLogCommand, string>
    {
        public async Task<string> Handle(CancelExperimentLogCommand request, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.FindAsync(el => el.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Experiment log not found");
            experimentLog.Cancel(request.Reason);
            experimentLog.UpdatedBy = currentUserService.UserId;
            experimentLog.UpdatedDate = DateTime.UtcNow;
            experimentLogRepository.Update(experimentLog);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? experimentLog.ID.ToString()
                : "Hủy thí nghiệm thất bại";
        }
    }
}
