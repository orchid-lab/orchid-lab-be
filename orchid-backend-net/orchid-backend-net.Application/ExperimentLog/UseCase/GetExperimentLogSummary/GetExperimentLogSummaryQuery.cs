using MediatR;
using orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.GetExperimentLogSummary
{
    /// <summary>
    /// <ul>
    /// <li>Dashboard tổng quan cho Researcher theo dõi một thí nghiệm cụ thể.</li>
    /// <li>Trả về: tỷ lệ sống, phân bố giai đoạn, số báo cáo chờ duyệt, sự cố bệnh.</li>
    /// </ul>
    /// </summary>
    public record GetExperimentLogSummaryQuery(string ExperimentLogId) : IRequest<ExperimentLogSummaryDto>;

    internal class GetExperimentLogSummaryQueryHandler(
        IExperimentLogRepository experimentLogRepository
    ) : IRequestHandler<GetExperimentLogSummaryQuery, ExperimentLogSummaryDto>
    {
        public async Task<ExperimentLogSummaryDto> Handle(
            GetExperimentLogSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var el = await experimentLogRepository.FindAsync(
                e => e.ID == request.ExperimentLogId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy thí nghiệm.");

            var samples = el.Samples;
            var totalSamples = samples.Count;
            var aliveSamples = samples.Count(s => !s.ExecutionDate.HasValue);
            var infectedSamples = samples.Count(s => s.ExecutionDate.HasValue);

            // Phân bố theo giai đoạn sinh học hiện tại của mẫu đang sống
            var stageDistribution = samples
                .Where(s => !s.ExecutionDate.HasValue)
                .GroupBy(s => s.SampleStages
                    .FirstOrDefault(st => st.Status == SampleStatus.InProgressed)
                    ?.SampleStageDefinition.Name ?? "Unknown")
                .Select(g => new SampleStageDistributionDto
                {
                    StageName = g.Key,
                    SampleCount = g.Count(),
                    Percentage = totalSamples > 0
                        ? Math.Round((double)g.Count() / totalSamples * 100, 1)
                        : 0
                })
                .ToList();

            // Monitoring logs pending
            var allMonitoringLogs = samples
                .SelectMany(s => s.SampleStages)
                .SelectMany(ss => ss.MonitoringLogs)
                .ToList();

            return new ExperimentLogSummaryDto
            {
                ExperimentLogId = el.ID,
                ExperimentLogName = el.Name,
                TotalSamples = totalSamples,
                ExpectedSamples = el.ExpectedSampleCount,
                AliveSamples = aliveSamples,
                InfectedSamples = infectedSamples,
                SurvivalRate = totalSamples > 0
                    ? Math.Round((double)aliveSamples / totalSamples * 100, 1) : 0,
                ProgressRate = el.ExpectedSampleCount > 0
                    ? Math.Round((double)aliveSamples / el.ExpectedSampleCount * 100, 1) : 0,
                StageDistribution = stageDistribution,
                TotalMonitoringLogs = allMonitoringLogs.Count,
                PendingApprovalLogs = allMonitoringLogs.Count(m =>
                    m.Status == MonitoringLogStatus.WaitingForApproval
                    || m.Status == MonitoringLogStatus.Revised),
                RejectedLogs = allMonitoringLogs.Count(m =>
                    m.Status == MonitoringLogStatus.Rejected),
            };
        }
    }
}
