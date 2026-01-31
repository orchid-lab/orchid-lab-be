using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.MonitoringLog.Dto.LogDetail;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.CreateMonitoringLog
{
    public record CreateMonitoringLogCommand(
        string Name,
        string SampleStageId,
        string AnalyticResultId,
        int DiseaseId,
        string Notes,
        List<AddLogDetailsDto> LogDetailsDtos)
        : IRequest<string>;

    internal class CreateMonitoringLogCommandHandler(
        ISampleStageRepository sampleStageRepository,
        IDiseaseRepository diseaseRepository,
        IAnalyticResultRepository analyticResultRepository,
        IStageRequirementDefinitionRepository stageRequirementDefinitionRepository,
        IMonitoringLogRepository monitoringLogRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<CreateMonitoringLogCommand, string>
    {
        public async Task<string> Handle(CreateMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            var sampleStage = await sampleStageRepository.FindSampleStageById(request.SampleStageId, cancellationToken);

            var disease = await diseaseRepository.FindAsync(r => r.ID == request.DiseaseId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy bệnh với ID đã cho.");

            var analyticResult = await analyticResultRepository.FindAsync(a => a.ID.Equals(request.AnalyticResultId), cancellationToken)
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
                IsNewest = true,
            };

            monitoringLogs.Created();

            foreach(var logDetailDto in request.LogDetailsDtos)
            {
                var stageRequirementDefinition = await stageRequirementDefinitionRepository
                    .FindStageRequirementDefinitionById(logDetailDto.StageRequirementDefinitionId, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy yêu cầu giai đoạn với ID đã cho.");
                
                bool isMatch = logDetailDto.MeasuredValue <= stageRequirementDefinition.MaxValue
                    && logDetailDto.MeasuredValue >= stageRequirementDefinition.MinValue;

                monitoringLogs.AddLogDetails(
                    logDetailDto.StageRequirementDefinitionId,
                    logDetailDto.MeasuredValue,
                    isMatch);
            }

            monitoringLogRepository.Add(monitoringLogs);
            return await monitoringLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? monitoringLogs.ID.ToString()
                : "Tạo thất bại";
        }
    }
}
