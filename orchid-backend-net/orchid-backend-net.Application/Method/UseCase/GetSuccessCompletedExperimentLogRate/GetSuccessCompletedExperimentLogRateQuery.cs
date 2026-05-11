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
        IMethodRepository methodRepository,
        IExperimentLogRepository experimentLogRepository
    ) :
        IRequestHandler<GetSuccessCompletedExperimentLogRateQuery, List<GetSuccessCompletedExperimentLogRateDto>>
    {
        public async Task<List<GetSuccessCompletedExperimentLogRateDto>> Handle(
            GetSuccessCompletedExperimentLogRateQuery request,
            CancellationToken cancellationToken)
        {
            // Get all methods from database
            var allMethods = await methodRepository.FindAllAsync(cancellationToken);

            // Get all experiment logs (only completed or failed ones for rate calculation)
            var allExperimentLogs = await experimentLogRepository.FindAllAsync(
                x => x.Status == ExperimentLogStatus.Completed || 
                     x.Status == ExperimentLogStatus.Destroyed || 
                     x.Status == ExperimentLogStatus.Cancelled,
                cancellationToken);

            // Build result for all methods
            var result = allMethods
                .Select(method =>
                {
                    // Get experiment logs for this specific method
                    var methodExperiments = allExperimentLogs
                        .Where(x => x.MethodId == method.ID)
                        .ToList();

                    // Count terminal states only for success rate calculation
                    var completedCount = methodExperiments.Count(x => x.Status == ExperimentLogStatus.Completed);
                    var failedCount = methodExperiments.Count(x => x.Status == ExperimentLogStatus.Destroyed ||
                                                                    x.Status == ExperimentLogStatus.Cancelled);

                    var totalTerminated = completedCount + failedCount;

                    // Calculate success rate: Completed / (Completed + Failed)
                    var successRate = totalTerminated > 0
                        ? (double)completedCount / totalTerminated * 100
                        : 0;

                    return new GetSuccessCompletedExperimentLogRateDto
                    {
                        Id = method.ID,
                        Name = method.Name,
                        Description = method.Description,
                        TotalDurationDays = method.MethodStages?.Sum(ms => ms.DurationsDays) ?? 0,
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
