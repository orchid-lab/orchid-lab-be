using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetail;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.ValueObjects;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.CreateMonitoringLog
{
    /// <summary>
    /// Creates monitoring log with log details.
    /// By default, submits immediately for researcher approval.
    /// Set submitImmediately=false to save as draft.
    /// </summary>
    public record CreateMonitoringLogCommand(
        string Name,
        string SampleStageId,
        string AnalyticResultId,
        int DiseaseId,
        string Notes,
        List<AddLogDetailsDto> LogDetailsDtos,
        bool SubmitImmediately = true)
        : IRequest<string>;

    internal class CreateMonitoringLogCommandHandler(
        ISampleStageRepository sampleStageRepository,
        IDiseaseRepository diseaseRepository,
        IAnalyticResultRepository analyticResultRepository,
        IStageRequirementDefinitionRepository stageRequirementDefinitionRepository,
        IMonitoringLogRepository monitoringLogRepository,
        IDiseaseIncidentRepository diseaseIncidentRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<CreateMonitoringLogCommand, string>
    {
        public async Task<string> Handle(CreateMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            // Load SampleStage with navigation properties to access ResearcherId
            var sampleStage = await sampleStageRepository.FindAsync(
                s => s.ID == request.SampleStageId,
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy sample stage.");

            var disease = await diseaseRepository.FindAsync(
                r => r.ID == request.DiseaseId, 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy bệnh với ID đã cho.");

            var analyticResult = await analyticResultRepository.FindAsync(
                a => a.ID.Equals(request.AnalyticResultId), 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy phân tích này");

            var monitoringLogs = new MonitoringLogs()
            {
                Name = request.Name,
                SampleStageId = sampleStage.ID,
                AnalyticResultId = analyticResult.ID,
                DiseaseId = disease.ID,
                Notes = request.Notes,
                UserId = currentUserService.UserId!,
                CreatedBy = currentUserService.UserId!,
                CreatedDate = DateTime.UtcNow,
                IsNewest = false, // Will be set to true only when approved
            };

            // Initialize as Created status
            monitoringLogs.Created();

            // Add all log details with validation
            foreach(var logDetailDto in request.LogDetailsDtos)
            {
                var stageRequirementDefinition = await stageRequirementDefinitionRepository
                    .FindStageRequirementDefinitionById(logDetailDto.StageRequirementDefinitionId, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy yêu cầu giai đoạn với ID đã cho.");
                
                // Create MeasurementRange value object
                var range = MeasurementRange.Create(
                    stageRequirementDefinition.MinValue,
                    stageRequirementDefinition.MaxValue);

                // Use new method with Value Object
                monitoringLogs.AddLogDetailsWithRange(
                    logDetailDto.StageRequirementDefinitionId,
                    logDetailDto.MeasuredValue,
                    range);
            }

            // Submit immediately if requested (default behavior)
            if (request.SubmitImmediately)
            {
                var researcherId = sampleStage.Samples.ExperimentLog.CreatedBy;
                
                if (string.IsNullOrWhiteSpace(researcherId))
                    throw new DomainException("Không tìm thấy researcher cho experiment log này.");

                monitoringLogs.SubmitForApproval(researcherId);
            }

            monitoringLogRepository.Add(monitoringLogs);

            var pendingIncident = await diseaseIncidentRepository.FindAsync(
                di => di.DiseaseId.Equals(disease.ID) &&
                di.SampleStageId.Equals(request.SampleStageId) &&
                di.MonitoringLogId == null &&
                di.Status.Equals(DiseaseIncidentStatus.AIDetected),
                cancellationToken);
            if(pendingIncident is not null)
            {
                pendingIncident.MonitoringLogId = monitoringLogs.ID;
                diseaseIncidentRepository.Update(pendingIncident);
            }

            var success = await monitoringLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            
            if (!success)
                return "Tạo thất bại";

            return request.SubmitImmediately
                ? $"Tạo và gửi báo cáo thành công. ID: {monitoringLogs.ID}"
                : $"Tạo báo cáo thành công (bản nháp). ID: {monitoringLogs.ID}";
        }
    }
}
