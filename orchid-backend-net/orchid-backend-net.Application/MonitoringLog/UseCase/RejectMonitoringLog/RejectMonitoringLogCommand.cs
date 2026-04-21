using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.RejectMonitoringLog
{
    /// <summary>
    /// Researcher rejects monitoring log and requests revision.
    /// Technician can then update log details and resubmit.
    /// </summary>
    public record RejectMonitoringLogCommand(
        string MonitoringLogId,
        string RejectionReason) : IRequest<string>;

    internal class RejectMonitoringLogCommandHandler(
        IMonitoringLogRepository monitoringLogRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<RejectMonitoringLogCommand, string>
    {
        public async Task<string> Handle(RejectMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            var monitoringLog = await monitoringLogRepository
                 .FindByIdWithResearcherAsync(request.MonitoringLogId, cancellationToken)
                 ?? throw new NotFoundException("Không tìm thấy monitoring log.");

            // Verify researcher ownership
            var researcherId = monitoringLog.SampleStage.Samples.ExperimentLog.CreatedBy;
            if (researcherId != currentUserService.UserId)
                throw new DomainException("Bạn không có quyền từ chối báo cáo này.");

            // Domain method handles validation, status transition, and event raising
            monitoringLog.Reject(currentUserService.UserId!, request.RejectionReason);

            monitoringLogRepository.Update(monitoringLog);
            
            return await monitoringLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Từ chối báo cáo thành công"
                : "Từ chối báo cáo thất bại";
        }
    }
}