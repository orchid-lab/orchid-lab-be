using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ExperimentLog.Dto.Report;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.ExportReport
{
    /// <summary>
    /// <ul>
    /// <li>Command duy nhất cho cả 2 loại report PDF. type = "process" hoặc "summary".</li>
    /// <li>Handler build model từ DB rồi gọi IPdfReportGenerator tương ứng.</li>
    /// </ul>
    /// </summary>
    public record ExportExperimentReportCommand(
        string ExperimentLogId,
        string ReportType // "process" | "summary"
    ) : IRequest<byte[]>;

    internal class ExportExperimentReportCommandHandler(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        ITaskRepository taskRepository,
        IPdfReportGenerator pdfReportGenerator) : IRequestHandler<ExportExperimentReportCommand, byte[]>
    {
        public async Task<byte[]> Handle(ExportExperimentReportCommand request, CancellationToken cancellationToken)
        {
            var el = await experimentLogRepository.GetForReportAsync(
                request.ExperimentLogId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy thí nghiệm.");

            var researcher = await userRepository.FindAsync(
                u => u.ID == el.CreatedBy, cancellationToken);
            var technician = await userRepository.FindAsync(
                u => u.ID == el.AssignedTo, cancellationToken);

            var tasks = await taskRepository.GetTasksByTargetAsync(
                TaskTargetType.ExperimentLog, el.ID, cancellationToken);

            var generatedAt = TimeZoneHelper.VietnamTimeNow.ToString("dd/MM/yyyy HH:mm");
            var researcherName = researcher?.Name ?? el.CreatedBy;
            var technicianName = technician?.Name ?? el.AssignedTo;

            if (request.ReportType.Equals("process", StringComparison.OrdinalIgnoreCase))
            {
                var model = BuildProcessLogModel(el, researcherName, technicianName, generatedAt, tasks);
                return await pdfReportGenerator.GenerateProcessLogAsync(model, cancellationToken);
            }
            else if (request.ReportType.Equals("summary", StringComparison.OrdinalIgnoreCase))
            {
                var model = BuildSummaryModel(el, researcherName, technicianName, generatedAt);
                return await pdfReportGenerator.GenerateSummaryReportAsync(model, cancellationToken);
            }

            throw new ArgumentException($"ReportType không hợp lệ: {request.ReportType}");
        }

        private static ExperimentProcessLogReportModel BuildProcessLogModel(
            ExperimentLogs el,
            string researcherName,
            string technicianName,
            string generatedAt,
            List<Domain.Entities.Tasks> tasks)
        {
            var samples = el.Samples ?? new List<Samples>();
            var totalSamples = samples.Count;
            var aliveSamples = samples.Count(s => !s.ExecutionDate.HasValue);
            var infectedSamples = samples.Count(s => s.ExecutionDate.HasValue);

            var methodStages = el.Method?.MethodStages
                .OrderBy(ms => ms.Order)
                .ToList() ?? new List<MethodStages>();

            var stageProgress = BuildStageProgress(samples, totalSamples);
            var methodStageTimeline = methodStages
                .Select(ms => BuildTimelineItem(ms, el, methodStages))
                .ToList();
            var aiResults = BuildAiAnalysisItems(samples);

            var totalTasks = tasks.Count;
            var tasksCompletedOnTime = tasks
                .Count(t => t.Status == Domain.Common.Enum.TaskStatus.CompletedInTime);
            var tasksCompletedLate = tasks
                .Count(t => t.Status == Domain.Common.Enum.TaskStatus.CompletedOutTime);


            return new ExperimentProcessLogReportModel
            {
                ExperimentName = el.Name,
                MethodName = el.Method?.Name ?? "Unknown",
                SeedlingLocalName = el.SeedlingParent?.LocalName ?? "Unknown",
                SeedlingScientificName = el.SeedlingParent?.ScientificName ?? "Unknown",
                ResearcherName = researcherName,
                TechnicianName = technicianName,
                StartDate = el.StartDate,
                EndDate = el.EndDate,
                GeneratedAt = generatedAt,
                TotalSamples = totalSamples,
                ExpectedSamples = el.ExpectedSampleCount,
                AliveSamples = aliveSamples,
                InfectedSamples = infectedSamples,
                SurvivalRate = totalSamples > 0
                    ? Math.Round((double)aliveSamples / totalSamples * 100, 1) : 0,
                StageProgress = stageProgress,
                MethodStageTimeline = methodStageTimeline,
                AIAnalysisResults = aiResults,
                DiseaseIncidents = BuildDiseaseIncidentItems(el.Samples),
                TotalTasks = totalTasks,
                TasksCompletedOnTime = tasksCompletedOnTime,
                TasksCompletedLate = tasksCompletedLate
            };
        }

        private static List<DiseaseIncidentReportItem> BuildDiseaseIncidentItems(List<Samples> samples)
            => samples
                .SelectMany(s => s.SampleStages
                    .SelectMany(ss => ss.DiseaseIncidents
                        .Select(di => new DiseaseIncidentReportItem
                        {
                            SampleName = s.Name,
                            DiseaseName = di.Disease?.Name ?? "Unknown",
                            AIConfidence = (double)di.AIConfidence * 100,
                            IncidentStatus = di.Status.ToString(),
                            ReviewNote = di.ReviewNote,
                        })))
                .ToList();

        private static List<SampleStageProgressItem> BuildStageProgress(
            List<Samples> samples, int totalSamples)
            => samples
                .Where(s => !s.ExecutionDate.HasValue)
                .GroupBy(s => s.SampleStages
                    .FirstOrDefault(st => st.Status == SampleStatus.InProgressed)
                    ?.SampleStageDefinition.Name ?? "Chưa xác định")
                .Select(g => new SampleStageProgressItem
                {
                    StageName = g.Key,
                    SampleCount = g.Count(),
                    Percentage = totalSamples > 0
                        ? Math.Round((double)g.Count() / totalSamples * 100, 1) : 0
                })
                .ToList();

        private static MethodStageTimelineItem BuildTimelineItem(
            MethodStages ms, ExperimentLogs el, List<MethodStages> allStages)
        {
            var (status, actualDays) = ResolveStageStatus(ms, el, allStages);
            return new MethodStageTimelineItem
            {
                StageOrder = ms.Order,
                StageName = ms.MethodStageDefinition?.Name ?? $"Stage {ms.Order}",
                PlannedDays = ms.DurationsDays,
                ActualDays = actualDays,
                Status = status
            };
        }

        private static (string status, int? actualDays) ResolveStageStatus(
            MethodStages ms, ExperimentLogs el, List<MethodStages> allStages)
        {
            if (ms.Order < el.CurrentStageOrder)
                return ("Completed", ms.DurationsDays);

            if (ms.Order > el.CurrentStageOrder)
                return ("Pending", null);

            // ms.Order == el.CurrentStageOrder
            var status = el.Status is ExperimentLogStatus.InProgress
                                   or ExperimentLogStatus.WaitingForChangeStage
                ? "InProgress" : "Completed";

            int? actualDays = null;
            if (el.StartDate.HasValue)
            {
                var elapsed = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber
                              - el.StartDate.Value.DayNumber;
                var prevDays = allStages
                    .Where(s => s.Order < ms.Order)
                    .Sum(s => s.DurationsDays);
                actualDays = Math.Max(0, elapsed - prevDays);
            }

            return (status, actualDays);
        }

        private static List<AIAnalysisItem> BuildAiAnalysisItems(List<Samples> samples)
            => samples
            .SelectMany(s => s.SampleStages
                .SelectMany(ss =>
                {
                    // Build lookup: MonitoringLogId → DiseaseIncident
                    var incidentByLogId = ss.DiseaseIncidents
                        .Where(di => di.MonitoringLogId != null)
                        .ToDictionary(di => di.MonitoringLogId!);

                    return ss.MonitoringLogs
                        .Where(ml => ml.AnalyticResult != null)
                        .Select(ml =>
                        {
                            incidentByLogId.TryGetValue(ml.ID, out var incident);
                            return new AIAnalysisItem
                            {
                                SampleName = s.Name,
                                StageName = ss.SampleStageDefinition?.Name ?? "Unknown",
                                DetectedDisease = ml.Disease?.Name ?? "Không phát hiện bệnh",
                                Confidence = (double)GetTopConfidence(ml.AnalyticResult!) * 100,
                                IncidentStatus = incident?.Status.ToString() ?? "NoIncident",
                                AnalyzedAt = ml.CreatedDate.ToVietnamTimeString()
                            };
                        });
                }))
            .ToList();

        private static ExperimentSummaryReportModel BuildSummaryModel(
            ExperimentLogs el,
            string researcherName,
            string technicianName,
            string generatedAt)
        {
            var samples = el.Samples ?? new List<Samples>();
            var totalSamples = samples.Count;
            var aliveSamples = samples.Count(s => !s.ExecutionDate.HasValue);
            var infectedSamples = samples.Count(s => s.ExecutionDate.HasValue);

            var methodStages = el.Method?.MethodStages
                .OrderBy(ms => ms.Order)
                .ToList() ?? new List<MethodStages>();

            var allMonitoringLogs = samples
                .SelectMany(s => s.SampleStages)
                .SelectMany(ss => ss.MonitoringLogs)
                .ToList();

            return new ExperimentSummaryReportModel
            {
                ExperimentName = el.Name,
                MethodName = el.Method?.Name ?? "Unknown",
                SeedlingLocalName = el.SeedlingParent?.LocalName ?? "Unknown",
                SeedlingScientificName = el.SeedlingParent?.ScientificName ?? "Unknown",
                ResearcherName = researcherName,
                TechnicianName = technicianName,
                StartDate = el.StartDate,
                EndDate = el.EndDate,
                GeneratedAt = generatedAt,
                CompletedDate = el.EndDate?.ToString("dd/MM/yyyy") ?? generatedAt,
                Objective = el.Objective,
                MethodStageTimeline = methodStages
                    .Select(ms => BuildTimelineItem(ms, el, methodStages))
                    .ToList(),
                TotalSamples = totalSamples,
                ExpectedSamples = el.ExpectedSampleCount,
                AliveSamples = aliveSamples,
                InfectedSamples = infectedSamples,
                SurvivalRate = totalSamples > 0
                    ? Math.Round((double)aliveSamples / totalSamples * 100, 1) : 0,
                FinalStageDistribution = BuildFinalStageDistribution(samples, totalSamples),
                TotalAIScans = allMonitoringLogs.Count(ml => ml.AnalyticResult != null),
                DiseasesDetected = CountDetectedDiseases(allMonitoringLogs),
                DiseasesConfirmedByHuman = 0,
                DiseasesDismissedByHuman = 0,
                TopDiseasesFound = GetTopDiseases(allMonitoringLogs),
                Conclusion = el.Conclusion,
                Issues = el.Issues,
                Recommendations = el.Recommendations,
                ResearcherSignature = researcherName
            };
        }

        private static List<SampleStageProgressItem> BuildFinalStageDistribution(
            List<Samples> samples, int totalSamples)
            => samples
                .Where(s => !s.ExecutionDate.HasValue)
                .GroupBy(s => s.SampleStages
                    .OrderByDescending(st => st.StartedAt)
                    .FirstOrDefault()
                    ?.SampleStageDefinition.Name ?? "Chưa xác định")
                .Select(g => new SampleStageProgressItem
                {
                    StageName = g.Key,
                    SampleCount = g.Count(),
                    Percentage = totalSamples > 0
                        ? Math.Round((double)g.Count() / totalSamples * 100, 1) : 0
                })
                .ToList();

        private static int CountDetectedDiseases(List<MonitoringLogs> logs)
            => logs.Count(ml =>
                ml.AnalyticResult != null
                && ml.Disease != null
                && ml.Disease.Code != "healthy");

        private static List<string> GetTopDiseases(List<MonitoringLogs> logs)
            => logs
                .Where(ml => ml.Disease != null && ml.Disease.Code != "healthy")
                .GroupBy(ml => ml.Disease!.Name)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

        private static decimal GetTopConfidence(AnalyticResults ar)
        {
            return new[]
            {
                ar.Anthracnose, ar.BacterialWilt, ar.Blackrot, ar.Brownspots,
                ar.MoldBacterial, ar.MoldFungus, ar.SoftRot, ar.StemRot,
                ar.WitheredYellowRoot, ar.Healthy, ar.Oxidation, ar.Virus
            }.Max();
        }
    }
}
