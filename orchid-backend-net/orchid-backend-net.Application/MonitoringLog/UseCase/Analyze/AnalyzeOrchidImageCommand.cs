using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.MonitoringLog.Dto.AnalyticResult;
using orchid_backend_net.Application.MonitoringLog.Dto.Disease;
using orchid_backend_net.Application.MonitoringLog.Helper;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.Analyze
{
    public record AnalyzeOrchidImageCommand(string FileName, byte[] FileStream, string? SampleStageId = null) : IRequest<AnalyticResultAfterAnalysisDto>;

    internal class AnalyzeOrchidImageCommandHandler(
        IOrchidAnalyzerService orchidAnalyzerService,
        IAnalyticResultRepository analyticResultRepository,
        IDiseaseRepository diseaseRepository,
        ISampleStageRepository sampleStageRepository,
        INotificationRepository notificationRepository,
        IDiseaseIncidentRepository diseaseIncidentRepository,
        INotificationPushService notificationPushService)
        : IRequestHandler<AnalyzeOrchidImageCommand, AnalyticResultAfterAnalysisDto>
    {
        public async Task<AnalyticResultAfterAnalysisDto> Handle(AnalyzeOrchidImageCommand request, CancellationToken cancellationToken)
        {
            // Run ONNX inference
            var analyticResult = await orchidAnalyzerService.AnalyzeAsync(request.FileStream, cancellationToken);

            if (analyticResult.Disease is null)
                throw new ArgumentException("Kết quả phân tích bệnh bị thiếu", nameof(request));

            var rawProbabilities = analyticResult.Disease.Probability ?? new Dictionary<string, float>();

            // Lấy danh sách bệnh đang active để ưu tiên kết quả hợp lệ trong business flow
            var activeDiseases = await diseaseRepository.FindAllAsync(
                d => d.IsActive && d.OnnxClassName != null,
                cancellationToken);

            var activeOnnxNames = activeDiseases
                .Select(d => d.OnnxClassName!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var activeProbabilities = rawProbabilities
                .Where(kvp => activeOnnxNames.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var selectedDiseaseName = activeProbabilities.Count > 0
                ? activeProbabilities.OrderByDescending(x => x.Value).First().Key
                : rawProbabilities.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault() ?? analyticResult.Disease.Predict;

            if (!string.IsNullOrWhiteSpace(selectedDiseaseName))
                analyticResult.Disease.Predict = selectedDiseaseName;

            // Validate stage name from ONNX (Coppice/Tissue/Tree)
            var stageName = OrchidAnalysisMapper.ValidateStageName(analyticResult.Stage);

            // Convert ONNX disease name → database code for lookup
            var diseaseCode = OrchidAnalysisMapper.ToDiseaseCode(analyticResult.Disease.Predict);

            // Lookup disease entity from database by code
            var analyticDisease = await diseaseRepository.FindProjectToAsync<DiseaseDto>(
                q => q.Where(d => d.Code.Equals(diseaseCode)),
                cancellationToken)
                ?? throw new NotFoundException($"Không tìm thấy bệnh với code: {diseaseCode}");

            // Map ONNX probabilities to AnalyticResults entity
            var analyticResultEntity = OrchidAnalysisMapper.ToAnalyticResult(analyticResult);
            analyticResultRepository.Add(analyticResultEntity);

            // Tự động tạo DiseaseIncident khi AI predict không phải "healthy"
            if (!string.IsNullOrWhiteSpace(request.SampleStageId)
                && !analyticDisease.Code.ToLower().Equals("healthy"))
            {
                var confidence = activeProbabilities.TryGetValue(analyticResult.Disease.Predict, out var activeConfidence)
                    ? activeConfidence
                    : rawProbabilities.GetValueOrDefault(analyticResult.Disease.Predict, 0f);

                var incident = new Domain.Entities.DiseaseIncident
                {
                    SampleStageId = request.SampleStageId,
                    MonitoringLogId = null,
                    DiseaseId = analyticDisease.Id,
                    AIConfidence = Convert.ToDecimal(confidence),
                    Status = Domain.Common.Enum.DiseaseIncidentStatus.AIDetected,
                    CreatedBy = "system",
                    CreatedDate = DateTime.UtcNow
                };
                diseaseIncidentRepository.Add(incident);
            }

            string? title = null;
            string? content = null;
            List<Domain.Entities.Notification>? notifications = null;
            List<string>? recipientIds = null;

            if (!string.IsNullOrWhiteSpace(request.SampleStageId))
            {
                var sampleStage = await sampleStageRepository.FindSampleStageById(request.SampleStageId, cancellationToken);
                var experimentLog = sampleStage.Samples.ExperimentLog;

                recipientIds = new[] { experimentLog.AssignedTo, experimentLog.CreatedBy }
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Distinct()
                    .ToList();

                if (recipientIds.Count > 0)
                {
                    title = "Kết quả AI phân tích mẫu";
                    content = $"Mẫu '{sampleStage.Samples.Name}' được phân tích: Stage '{stageName}', bệnh '{analyticDisease.Name}'.";

                    notifications = recipientIds
                        .Select(userId => CreateNotificationHelper.CreateForSingleUsers(
                            userId, title, content,
                            Domain.Common.Enum.NotificationTargetType.Sample,
                            sampleStage.SampleId.ToString()))
                        .ToList();

                    notificationRepository.AddRange(notifications);
                }
            }

            // Build response DTO
            var resultObject = new AnalyticResultAfterAnalysisDto
            {
                StageName = stageName,
                Disease = analyticDisease,
                AnalyticResult = AnalyticResultDto.Create(analyticResultEntity)
            };

            var isSaved = await analyticResultRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            if (!isSaved)
                throw new InvalidOperationException("Phân tích thất bại");

            if (notifications is { Count: > 0 } && recipientIds is { Count: > 0 } && title is not null && content is not null)
            {
                await notificationPushService.PushToMultipleUserAsync(recipientIds, title, content);
            }

            return resultObject;
        }
    }
}