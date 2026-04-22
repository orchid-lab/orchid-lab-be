using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.ValueObjects;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.UpdateMonitoringLogDetail
{
    /// <summary>
    /// DTO for updating individual log detail.
    /// </summary>
    public record UpdateLogDetailDto(
        string LogDetailId,
        decimal MeasuredValue);

    /// <summary>
    /// Technician updates log details after rejection.
    /// Can only update when status is Rejected.
    /// After updating, technician must resubmit for approval.
    /// </summary>
    public record UpdateMonitoringLogDetailCommand(
        string MonitoringLogId,
        List<UpdateLogDetailDto> UpdatedLogDetails) : IRequest<string>;

    internal class UpdateMonitoringLogDetailCommandHandler(
        IMonitoringLogRepository monitoringLogRepository,
        IStageRequirementDefinitionRepository stageRequirementDefinitionRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<UpdateMonitoringLogDetailCommand, string>
    {
        public async Task<string> Handle(UpdateMonitoringLogDetailCommand request, CancellationToken cancellationToken)
        {
            var monitoringLog = await monitoringLogRepository
                .FindByIdWithLogDetailsAsync(request.MonitoringLogId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy monitoring log.");

            // Verify technician ownership
            if (monitoringLog.UserId != currentUserService.UserId)
                throw new DomainException("Bạn không có quyền cập nhật báo cáo này.");

            foreach (var updateDto in request.UpdatedLogDetails)
            {
                var logDetail = monitoringLog.LogDetails.FirstOrDefault(ld => ld.ID == updateDto.LogDetailId)
                    ?? throw new NotFoundException($"Không tìm thấy log detail với ID {updateDto.LogDetailId}.");

                // Get stage requirement for validation
                var stageRequirement = await stageRequirementDefinitionRepository
                    .FindStageRequirementDefinitionById(logDetail.StageRequirementDefinitionId, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy stage requirement definition.");

                // Create MeasurementRange value object
                var range = MeasurementRange.Create(
                    stageRequirement.MinValue,
                    stageRequirement.MaxValue);

                // Use domain method with Value Object (validation logic is now in domain)
                monitoringLog.UpdateLogDetailWithRange(
                    updateDto.LogDetailId,
                    updateDto.MeasuredValue,
                    range);
            }

            monitoringLogRepository.Update(monitoringLog);
            
            return await monitoringLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Cập nhật báo cáo thành công. Vui lòng gửi lại để duyệt."
                : "Cập nhật báo cáo thất bại";
        }
    }
}