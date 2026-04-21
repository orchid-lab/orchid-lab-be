using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetail;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Common.Interfaces;
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
        CreateMonitoringLogRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
        : IRequestHandler<CreateMonitoringLogCommand, string>
    {
        public async Task<string> Handle(CreateMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            // Load SampleStage with navigation properties to access ResearcherId
            var sampleStage = await repository.SampleStageRepository.FindAsync(
                s => s.ID == request.SampleStageId,
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy sample stage.");

            var disease = await repository.DiseaseRepository.FindAsync(
                r => r.ID == request.DiseaseId, 
                cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy bệnh với ID đã cho.");

            var analyticResult = await repository.AnalyticResultRepository.FindAsync(
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
                var stageRequirementDefinition = await repository.StageRequirementDefinitionRepository
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

            repository.MonitoringLogRepository.Add(monitoringLogs);

            var pendingIncident = await repository.DiseaseIncidentRepository.FindAsync(
                di => di.DiseaseId.Equals(disease.ID) &&
                di.SampleStageId.Equals(request.SampleStageId) &&
                di.MonitoringLogId == null &&
                di.Status.Equals(DiseaseIncidentStatus.AIDetected),
                cancellationToken);
            if(pendingIncident is not null)
            {
                pendingIncident.MonitoringLogId = monitoringLogs.ID;
                repository.DiseaseIncidentRepository.Update(pendingIncident);
            }

            var success = await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
            
            if (!success)
                return "Tạo thất bại";

            return request.SubmitImmediately
                ? $"Tạo và gửi báo cáo thành công. ID: {monitoringLogs.ID}"
                : $"Tạo báo cáo thành công (bản nháp). ID: {monitoringLogs.ID}";
        }
    }

    public sealed class CreateMonitoringLogRepository(
        ISampleStageRepository sampleStageRepository,
        IDiseaseRepository diseaseRepository,
        IAnalyticResultRepository analyticResultRepository,
        IStageRequirementDefinitionRepository stageRequirementDefinitionRepository,
        IMonitoringLogRepository monitoringLogRepository,
        IDiseaseIncidentRepository diseaseIncidentRepository)
    {
        public ISampleStageRepository SampleStageRepository { get; } = sampleStageRepository;
        public IDiseaseRepository DiseaseRepository { get; } = diseaseRepository;
        public IAnalyticResultRepository AnalyticResultRepository { get; } = analyticResultRepository;
        public IStageRequirementDefinitionRepository StageRequirementDefinitionRepository { get; } = stageRequirementDefinitionRepository;
        public IMonitoringLogRepository MonitoringLogRepository { get; } = monitoringLogRepository;
        public IDiseaseIncidentRepository DiseaseIncidentRepository { get; } = diseaseIncidentRepository;
    }
}
