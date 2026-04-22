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

            //Validate stage name from ONNX (Coppice/Tissue/Tree)
            var stageName = OrchidAnalysisMapper.ValidateStageName(analyticResult.Stage);

            //Convert ONNX disease name → database code for lookup
            // e.g., "Anthracnose" → "disease_anthracnose"
            var diseaseCode = OrchidAnalysisMapper.ToDiseaseCode(analyticResult.Disease.Predict);

            // Lookup disease entity from database by code
            var analyticDisease = await diseaseRepository.FindProjectToAsync<DiseaseDto>(
                q => q.Where(d => d.Code.Equals(diseaseCode)),
                cancellationToken)
                ?? throw new NotFoundException($"Không tìm thấy bệnh với code: {diseaseCode}");

            // Map ONNX probabilities to AnalyticResults entity
            var analyticResultEntity = OrchidAnalysisMapper.ToAnalyticResult(analyticResult);
            analyticResultRepository.Add(analyticResultEntity);

            // Mục đích: Tự động tạo DiseaseIncident khi AI predict không phải "healthy"
            // Chỉ chạy khi SampleStageId được cung cấp (có context mẫu vật cụ thể)
            if (!string.IsNullOrWhiteSpace(request.SampleStageId)
                && !analyticDisease.Code.ToLower().Equals("healthy"))
            {
                var confidence = analyticResult.Disease.Probability
                    .GetValueOrDefault(analyticResult.Disease.Predict, 0f);

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
                        .Select(userId => CreateNotificationHelper.CreateForSingleUsers(userId, title, content))
                        .ToList();

                    notificationRepository.AddRange(notifications);
                }
            }

            // Build response DTO
            var resultObject = new AnalyticResultAfterAnalysisDto
            {
                StageName = stageName,  // ✅ UPDATED #3: Use validated stage name
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
