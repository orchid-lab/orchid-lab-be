using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetSuccessCompletedExperimentLogRate
{
    public class GetSuccessCompletedExperimentLogRateQuery : IRequest<List<GetSuccessCompletedExperimentLogRateDto>>
    {
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        public GetSuccessCompletedExperimentLogRateQuery() { }
        //public GetSuccessCompletedExperimentLogRateQuery(int pageNumber, int pageSize)
        //{
        //    PageNumber = pageNumber;
        //    PageSize = pageSize;
        //}
    }
    internal class GetSuccessCompletedExperimentLogRateQueryHandler(
        IMethodRepository methodRepository,
        IExperimentLogRepository experimentLogRepository,
        IMethodStageDefinitionRepository methodStageDefinitionRepository,
        IMethodStageRepository methodStageRepository) :
        IRequestHandler<GetSuccessCompletedExperimentLogRateQuery, List<GetSuccessCompletedExperimentLogRateDto>>
    {
        public async Task<List<GetSuccessCompletedExperimentLogRateDto>> Handle(GetSuccessCompletedExperimentLogRateQuery request, CancellationToken cancellationToken)
        {
            var result = await experimentLogRepository.FindAllAsync(
                cancellationToken);

            var dtoTasks = result
                .GroupBy(x => new { x.Method, x.ID })
                .Select(async g =>
                {
                    var method = await methodRepository.FindAsync(x => x.ID.Equals(g.Key.Method.ID), cancellationToken);
                    var completedExperimentLog = g.Count(x => x.Status == ExperimentLogStatus.Completed);
                    var failedExperimentLog = g.Count(x => x.Status == ExperimentLogStatus.Cancelled);
                    List<Domain.Entities.MethodStageDefinition> methodStages = new List<Domain.Entities.MethodStageDefinition>();
                    foreach (var item in g.Where(x => x.Status == ExperimentLogStatus.Cancelled).ToList())
                    {
                        var methodStage = await methodStageRepository.FindAsync(x => x.ID.Equals(item.CurrentStageOrder), cancellationToken);
                        methodStages.Add(await methodStageDefinitionRepository.FindAsync(x => x.ID.Equals(methodStage.MethodStageDefinitionId), cancellationToken));
                    }
                    //var successRate = completedExperimentLog + failedExperimentLog > 0 ? (int)((double)completedExperimentLog / (completedExperimentLog + failedExperimentLog) * 100) : 0;
                    return new GetSuccessCompletedExperimentLogRateDto
                    {
                        Id = method.ID,
                        Name = method.Name,
                        Description = method.Description,
                        CompletedExperimentLog = completedExperimentLog,
                        FailedExperimentLog = failedExperimentLog,
                        SuccessRate = completedExperimentLog + failedExperimentLog > 0 ? (int)((double)completedExperimentLog / (completedExperimentLog + failedExperimentLog) * 100) : 0,
                        MethodStages = methodStages,
                    };
                });
            var dtoList = await Task.WhenAll(dtoTasks);
            return dtoList
                .OrderByDescending(r => r.SuccessRate)
                .ToList();
        }
    }
}
