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
            var rawTopDisease = rawProbabilities
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .FirstOrDefault() ?? analyticResult.Disease.Predict;

            // Lấy danh sách disease từ DB để mapping và kiểm tra trạng thái (active/inactive)
            var allDiseases = await diseaseRepository.FindAllAsync(
                d => d.OnnxClassName != null,
                cancellationToken);

            // Normalization helper
            static string NormalizeName(string s)
            {
                if (string.IsNullOrWhiteSpace(s)) return string.Empty;
                return System.Text.RegularExpressions.Regex.Replace(s.ToLowerInvariant(), "[^a-z0-9]", string.Empty);
            }

            var dbNorm = allDiseases
                .Select(d => new { Onnx = d.OnnxClassName!, Norm = NormalizeName(d.OnnxClassName!), IsActive = d.IsActive, Code = d.Code, Id = d.ID, Name = d.Name })
                .ToList();

            // Check if model rawTopDisease corresponds to any DB OnnxClassName (flexible matching)
            string rawTopNorm = NormalizeName(rawTopDisease);
            var matchForRawTop = dbNorm.FirstOrDefault(a => string.Equals(a.Onnx, rawTopDisease, StringComparison.OrdinalIgnoreCase)
                                                            || rawTopNorm == a.Norm
                                                            || rawTopNorm.Contains(a.Norm)
                                                            || a.Norm.Contains(rawTopNorm));

            bool isRawTopDiseaseActive = matchForRawTop != null && matchForRawTop.IsActive;

            string selectedDiseaseName;
            string matchedDbOnnxName = null;
            if (isRawTopDiseaseActive)
            {
                matchedDbOnnxName = matchForRawTop.Onnx;
                selectedDiseaseName = matchedDbOnnxName ?? rawTopDisease;
            }
            else
            {
                selectedDiseaseName = "Unknown";
            }

            // Validate stage name from ONNX (Coppice/Tissue/Tree)
            var stageName = OrchidAnalysisMapper.ValidateStageName(analyticResult.Stage);

            DiseaseDto analyticDisease;
            if (selectedDiseaseName.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
            {
                analyticDisease = new DiseaseDto
                {
                    Id = 0,
                    Name = "Unknown",
                    Code = "unknown",
                    Description = string.IsNullOrWhiteSpace(rawTopDisease)
                        ? "Kết quả không xác định."
                        : $"Bệnh '{rawTopDisease}' hiện không active trong hệ thống.",
                    OnnxClassName = rawTopDisease,
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                };
            }
            else
            {
                // Map matched DB OnnxClassName → Disease.Code
                var diseaseCode = OrchidAnalysisMapper.ToDiseaseCode(selectedDiseaseName);

                analyticDisease = await diseaseRepository.FindProjectToAsync<DiseaseDto>(
                    q => q.Where(d => d.Code.Equals(diseaseCode)),
                    cancellationToken)
                    ?? throw new NotFoundException($"Không tìm thấy bệnh với code: {diseaseCode}");
            }

            // Map ONNX probabilities to AnalyticResults entity
            var analyticResultEntity = OrchidAnalysisMapper.ToAnalyticResult(analyticResult);
            analyticResultRepository.Add(analyticResultEntity);

            // Prepare response AnalyticResultDto — mark inactive only when DB mapping exists and IsActive == false
            var analyticResultDto = AnalyticResultDto.Create(analyticResultEntity);

            var fullPredictions = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, decimal>>(analyticResultEntity.PredictionsJson)
                                  ?? new Dictionary<string, decimal>();

            var displayedPredictions = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in fullPredictions)
            {
                var modelName = kvp.Key;

                // Build friendly display name from model label (strip 'disease_' prefix and PascalCase tokens)
                string baseName = modelName;
                if (baseName.StartsWith("disease_", StringComparison.OrdinalIgnoreCase))
                    baseName = baseName.Substring("disease_".Length);

                var tokens = System.Text.RegularExpressions.Regex.Split(baseName, "[^A-Za-z0-9]+")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToArray();

                string displayName;
                if (tokens.Length == 0)
                    displayName = modelName;
                else
                    displayName = string.Concat(tokens.Select(t => char.ToUpperInvariant(t[0]) + (t.Length > 1 ? t.Substring(1) : string.Empty)));

                // find DB mapping for this model label (if any)
                var match = dbNorm.FirstOrDefault(a => string.Equals(a.Onnx, modelName, StringComparison.OrdinalIgnoreCase)
                                                      || NormalizeName(a.Onnx) == NormalizeName(modelName)
                                                      || NormalizeName(modelName).Contains(a.Norm)
                                                      || a.Norm.Contains(NormalizeName(modelName)));

                // Only mark inactive when there's a DB mapping and it's inactive
                var isActive = match != null ? match.IsActive : true;
                var displayKey = isActive ? displayName : $"{displayName} (inactive)";

                // Avoid duplicate keys by appending a numeric suffix if needed
                var keyToUse = displayKey;
                var suffix = 1;
                while (displayedPredictions.ContainsKey(keyToUse))
                {
                    keyToUse = displayKey + "_" + suffix++;
                }

                displayedPredictions[keyToUse] = kvp.Value;
            }

            analyticResultDto.Predictions = displayedPredictions;

            analyticResultDto.TopDisease = selectedDiseaseName;
            analyticResultDto.Confidence = selectedDiseaseName.Equals("Unknown", StringComparison.OrdinalIgnoreCase)
                ? 0m
                : analyticResultDto.Confidence;

            // Tự động tạo DiseaseIncident khi AI predict không phải "healthy"
            if (!string.IsNullOrWhiteSpace(request.SampleStageId)
                && !selectedDiseaseName.Equals("Unknown", StringComparison.OrdinalIgnoreCase)
                && !analyticDisease.Code.ToLower().Equals("healthy"))
            {
                var confidence = rawProbabilities.GetValueOrDefault(rawTopDisease, 0f);

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
                AnalyticResult = analyticResultDto,
                RawTopDisease = rawTopDisease,
                SelectedDisease = selectedDiseaseName,
                IsRawTopDiseaseActive = isRawTopDiseaseActive
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