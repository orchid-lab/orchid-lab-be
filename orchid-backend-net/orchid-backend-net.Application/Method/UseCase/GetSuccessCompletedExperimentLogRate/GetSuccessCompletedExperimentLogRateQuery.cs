using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetSuccessCompletedExperimentLogRate
{
    public class GetSuccessCompletedExperimentLogRateQuery : IRequest<List<GetSuccessCompletedExperimentLogRateDto>>
    {
        public GetSuccessCompletedExperimentLogRateQuery() { }
    }

    internal class GetSuccessCompletedExperimentLogRateQueryHandler(
        IMethodRepository methodRepository,
        IExperimentLogRepository experimentLogRepository,
        IMethodStageRepository methodStageRepository,
        IMethodStageDefinitionRepository methodStageDefinitionRepository,
        ISeedlingRepository seedlingRepository
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
            //var allExperimentLogs = await experimentLogRepository.FindAllAsync(
            //    x => x.Status == ExperimentLogStatus.Completed || 
            //         x.Status == ExperimentLogStatus.Destroyed || 
            //         x.Status == ExperimentLogStatus.Cancelled,
            //    cancellationToken);

            //// Build result for all methods
            //var result = allMethods
            //    .Select(method =>
            //    {
            //        // Get experiment logs for this specific method
            //        var methodExperiments = allExperimentLogs
            //            .Where(x => x.MethodId == method.ID)
            //            .ToList();

            //        // Count terminal states only for success rate calculation
            //        var completedCount = methodExperiments.Count(x => x.Status == ExperimentLogStatus.Completed);
            //        var failedCount = methodExperiments.Count(x => x.Status == ExperimentLogStatus.Destroyed ||
            //                                                        x.Status == ExperimentLogStatus.Cancelled);

            //        var totalTerminated = completedCount + failedCount;

            //        // Calculate success rate: Completed / (Completed + Failed)
            //        var successRate = totalTerminated > 0
            //            ? (double)completedCount / totalTerminated * 100
            //            : 0;

            //        return new GetSuccessCompletedExperimentLogRateDto
            //        {
            //            Id = method.ID,
            //            Name = method.Name,
            //            Description = method.Description,
            //            TotalDurationDays = method.MethodStages?.Sum(ms => ms.DurationsDays) ?? 0,
            //            CompletedExperimentLog = completedCount,
            //            FailedExperimentLog = failedCount,
            //            SuccessRate = successRate,
            //            TotalExperimentLog = totalTerminated
            //        };
            //    })
            //    .OrderByDescending(r => r.SuccessRate)
            //    .ToList();

            //return result;
            List<GetSuccessCompletedExperimentLogRateDto> result = new List<GetSuccessCompletedExperimentLogRateDto>();
            foreach (var item in allMethods)
            {
                var methodExperiments = await experimentLogRepository.FindAllAsync(x => x.MethodId == item.ID,cancellationToken);
                var completedCount = methodExperiments.Count(x => x.Status == ExperimentLogStatus.Completed && x.MethodId.Equals(item.ID));
                var failedCount = methodExperiments.Count(x => x.MethodId.Equals(item.ID) && (x.Status == ExperimentLogStatus.Destroyed ||
                                                                x.Status == ExperimentLogStatus.Cancelled));
                var inProcessCount = methodExperiments.Count(x => x.MethodId.Equals(item.ID) && (x.Status == ExperimentLogStatus.Created ||
                                                        x.Status == ExperimentLogStatus.InProgress ||
                                                        x.Status == ExperimentLogStatus.WaitingForChangeStage ||
                                                        x.Status == ExperimentLogStatus.ConfirmChangeStage));
                var totalTerminated = completedCount + failedCount + inProcessCount;
                var successRate = (completedCount + failedCount) > 0
                    ? (double)completedCount / (completedCount + failedCount) * 100
                    : 0;

                List<Seedlings> seedlingList = new List<Seedlings>();
                List<Domain.Entities.MethodStageDefinition> methodStageDefinitionList = new List<Domain.Entities.MethodStageDefinition>();
                foreach (var experiment in methodExperiments)
                {
                    var methodStages = await methodStageRepository.FindAsync(x => x.MethodId == experiment.CurrentStageOrder, cancellationToken);
                    if (methodStages != null) 
                    {
                        var methodStageDefinitions = await methodStageDefinitionRepository.FindAsync(x => x.ID == methodStages.MethodStageDefinitionId, cancellationToken);
                        if (methodStageDefinitions != null) 
                        { 
                            methodStageDefinitionList.Add(methodStageDefinitions);
                            seedlingList.Add(await seedlingRepository.FindAsync(x => x.ID == experiment.SeedlingParentId, cancellationToken));
                        }
                    }
                }
                result.Add(new GetSuccessCompletedExperimentLogRateDto 
                {
                    Id = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    TotalDurationDays = item.MethodStages?.Sum(ms => ms.DurationsDays) ?? 0,
                    CompletedExperimentLog = completedCount,
                    FailedExperimentLog = failedCount,
                    SuccessRate = successRate,
                    TotalExperimentLog = totalTerminated,
                    MethodStages = methodStageDefinitionList,
                    Seedlings = seedlingList
                });
            }
            return result;
        }
    }
}
