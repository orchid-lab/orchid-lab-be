using MediatR;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.ChangeSampleStage
{
    public record ChangeSampleStageCommand(string SampleId) : IRequest<string>;

    internal class ChangeSampleStageCommandHandler(
        ISampleRepository sampleRepository,
        ISampleStageDefinitionRepository sampleStageDefinitionRepository,
        IMonitoringLogRepository monitoringLogRepository,
        IStageRequirementDefinitionRepository stageRequirementDefinitionRepository,
        ILogger<ChangeSampleStageCommandHandler> logger)
        : IRequestHandler<ChangeSampleStageCommand, string>
    {
        public async Task<string> Handle(ChangeSampleStageCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(
                s => s.ID.Equals(request.SampleId),
                cancellationToken)
                ?? throw new NotFoundException("Sample không tồn tại");

            // Guard: sample bị nhiễm / đã hủy thì không được chuyển stage
            if (sample.ExecutionDate.HasValue)
                throw new DomainException("Sample đã bị hủy do bệnh, không thể chuyển giai đoạn.");

            var currentStage = sample.SampleStages
                .SingleOrDefault(s => s.Status == SampleStatus.InProgressed)
                ?? throw new DomainException("Sample không có giai đoạn đang tiến hành.");

            if (currentStage.Status == SampleStatus.ExecutedBecauseOfDisease)
                throw new DomainException("Sample đã bị hủy do bệnh, không thể chuyển giai đoạn.");

            // Lấy báo cáo approved mới nhất của chính sample stage hiện tại (với LogDetails)
            var latestApprovedLog = await monitoringLogRepository
                .FindLatestApprovedLogWithDetailsBySampleStageIdAsync(currentStage.ID, cancellationToken);

            if (latestApprovedLog is null)
                throw new DomainException("Chưa có báo cáo giám sát đã duyệt cho giai đoạn hiện tại.");

            // Lấy toàn bộ quy cách bắt buộc của sample stage definition hiện tại
            var requirements = await stageRequirementDefinitionRepository.FindAllAsync(
                r => r.SampleStageDefinitionId == currentStage.SampleStageDefinitionId,
                cancellationToken);

            if (requirements.Count == 0)
                throw new DomainException("Giai đoạn hiện tại chưa được cấu hình quy cách đánh giá.");

            var requiredIds = requirements.Select(r => r.ID).ToHashSet();

            // Thiếu chỉ số bắt buộc trong log detail
            var providedIds = latestApprovedLog.LogDetails
                .Select(d => d.StageRequirementDefinitionId)
                .ToHashSet();

            var missingRequirementCount = requiredIds.Count(id => !providedIds.Contains(id));
            if (missingRequirementCount > 0)
                throw new DomainException("Báo cáo giám sát chưa đủ chỉ số quy cách bắt buộc.");

            // Có chỉ số ngoài ngưỡng
            var hasOutOfRangeMetric = latestApprovedLog.LogDetails
                .Any(d => requiredIds.Contains(d.StageRequirementDefinitionId) && !d.IsMatch);

            if (hasOutOfRangeMetric)
                throw new DomainException("Có chỉ số sinh học chưa đạt ngưỡng quy cách, chưa thể chuyển giai đoạn.");

            logger.LogInformation("Sample {SampleId} passed all biological eligibility checks.", sample.ID);

            var orderedStageDefinitionIds = await sampleStageDefinitionRepository
                .GetOrderDefinitionIdsAsync(cancellationToken);

            sample.CompleteCurrentStage(orderedStageDefinitionIds);

            sampleRepository.Update(sample);

            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? sample.ID.ToString()
                : "Chuyển giai đoạn sample thất bại";
        }
    }
}
