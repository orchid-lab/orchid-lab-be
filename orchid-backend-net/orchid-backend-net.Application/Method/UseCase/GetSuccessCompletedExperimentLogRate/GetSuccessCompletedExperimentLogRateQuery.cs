using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetSuccessCompletedExperimentLogRate
{
    public class GetSuccessCompletedExperimentLogRateQuery : IRequest<List<GetSuccessCompletedExperimentLogRateDto>>
    {
        public GetSuccessCompletedExperimentLogRateQuery() { }
    }

    internal class GetSuccessCompletedExperimentLogRateQueryHandler(
        IExperimentLogRepository experimentLogRepository
    ) :
        IRequestHandler<GetSuccessCompletedExperimentLogRateQuery, List<GetSuccessCompletedExperimentLogRateDto>>
    {
        public async Task<List<GetSuccessCompletedExperimentLogRateDto>> Handle(
            GetSuccessCompletedExperimentLogRateQuery request,
            CancellationToken cancellationToken)
        {
            var allExperimentLogs = await experimentLogRepository.FindAllAsync(cancellationToken);

            // Group by Method to calculate success rate per method
            var result = allExperimentLogs
                .GroupBy(x => x.Method)
                .Select(g =>
                {
                    // Count terminal states only for success rate calculation
                    var completedCount = g.Count(x => x.Status == ExperimentLogStatus.Completed);
                    var failedCount = g.Count(x => x.Status == ExperimentLogStatus.Destroyed ||
                                                    x.Status == ExperimentLogStatus.Cancelled);
                    
                    var totalTerminated = completedCount + failedCount;

                    // Calculate success rate: Completed / (Completed + Failed)
                    var successRate = totalTerminated > 0
                        ? (double)completedCount / totalTerminated * 100
                        : 0;

                    // Safely access method properties with null-coalescing
                    var method = g.Key;
                    return new GetSuccessCompletedExperimentLogRateDto
                    {
                        Id = method?.ID ?? 0,
                        Name = method?.Name ?? "Unknown",
                        Description = method?.Description ?? "Unknown",
                        TotalDurationDays = method?.MethodStages?.Sum(ms => ms.DurationsDays) ?? 0,
                        CompletedExperimentLog = completedCount,
                        FailedExperimentLog = failedCount,
                        SuccessRate = successRate,
                        TotalExperimentLog = totalTerminated
                    };
                })
                .OrderByDescending(r => r.SuccessRate)
                .ToList();

            return result;
        }
    }
}
