using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.ApproveMonitoringLog
{
    /// <summary>
    /// Researcher approves monitoring log.
    /// <ul>
    /// <li>Sets approved log's IsNewest = true</li>
    /// <li>Sets all other approved logs for same sample stage IsNewest = false</li>
    /// <li>Ensures only one approved log is newest per sample stage</li>
    /// </ul>
    /// </summary>
    public record ApproveMonitoringLogCommand(string MonitoringLogId) : IRequest<string>;

    internal class ApproveMonitoringLogCommandHandler(
        IMonitoringLogRepository monitoringLogRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<ApproveMonitoringLogCommand, string>
    {
        public async Task<string> Handle(ApproveMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            var monitoringLog = await monitoringLogRepository
                .FindByIdWithResearcherAsync(request.MonitoringLogId, cancellationToken)
                 ?? throw new NotFoundException("Không tìm thấy monitoring log.");

            // Verify researcher ownership
            var researcherId = monitoringLog.SampleStage.Samples.ExperimentLog.CreatedBy;
            if (researcherId != currentUserService.UserId)
                throw new DomainException("Bạn không có quyền duyệt báo cáo này.");

            // Set IsNewest = false for all other approved logs of the same sample stage
            var otherApprovedLogs = await monitoringLogRepository.FindAllAsync(
                m => m.SampleStageId == monitoringLog.SampleStageId 
                     && m.ID != monitoringLog.ID 
                     && m.Status == MonitoringLogStatus.Approved
                     && m.IsNewest,
                cancellationToken);

            foreach (var log in otherApprovedLogs)
            {
                log.IsNewest = false;
            }

            // Approve current log (sets IsNewest = true and raises domain event)
            monitoringLog.Approve(currentUserService.UserId!);

            monitoringLogRepository.UpdateRange(otherApprovedLogs);
            monitoringLogRepository.Update(monitoringLog);
            
            return await monitoringLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Duyệt báo cáo thành công"
                : "Duyệt báo cáo thất bại";
        }
    }
}