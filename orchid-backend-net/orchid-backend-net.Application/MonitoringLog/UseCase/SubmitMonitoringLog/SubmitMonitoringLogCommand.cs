using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.SubmitMonitoringLog
{
    /// <summary>
    /// Technician submits monitoring log for researcher approval.
    /// Used for:
    /// <ul>
    /// <li>Manual submission of draft (Created status)</li>
    /// <li>Resubmission after rejection (Rejected status)</li>
    /// </ul>
    /// </summary>
    public record SubmitMonitoringLogCommand(string MonitoringLogId) : IRequest<string>;

    internal class SubmitMonitoringLogCommandHandler(
        IMonitoringLogRepository monitoringLogRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<SubmitMonitoringLogCommand, string>
    {
        public async Task<string> Handle(SubmitMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            var monitoringLog = await monitoringLogRepository
                .FindByIdWithResearcherAsync(request.MonitoringLogId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy monitoring log.");

            // Verify technician ownership
            if (monitoringLog.UserId != currentUserService.UserId)
                throw new DomainException("Bạn không có quyền gửi báo cáo này.");

            // Get researcher from ExperimentLog via navigation
            var researcherId = monitoringLog.SampleStage.Samples.ExperimentLog.CreatedBy;
            
            if (string.IsNullOrWhiteSpace(researcherId))
                throw new DomainException("Không tìm thấy researcher cho experiment log này.");

            monitoringLog.SubmitForApproval(researcherId);

            monitoringLogRepository.Update(monitoringLog);
            
            return await monitoringLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Gửi báo cáo thành công"
                : "Gửi báo cáo thất bại";
        }
    }
}