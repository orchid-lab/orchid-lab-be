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
        IExperimentLogRepository experimentLogRepository,
        IMethodStageDefinitionRepository methodStageDefinitionRepository,
        IMethodStageRepository methodStageRepository) :
        IRequestHandler<GetSuccessCompletedExperimentLogRateQuery, List<GetSuccessCompletedExperimentLogRateDto>>
    {
        public async Task<List<GetSuccessCompletedExperimentLogRateDto>> Handle(GetSuccessCompletedExperimentLogRateQuery request, CancellationToken cancellationToken)
        {
            var result = await experimentLogRepository.FindAllAsync(
                cancellationToken);

            var dto = result
                .GroupBy(x => new { x.Method, x.ID })
                .Select(async g =>
                {
                    //var totalExperimentLogs = g.Count();
                    var completedExperimentLog = g.Count(x => x.Status == ExperimentLogStatus.Completed);
                    var failedExperimentLog = g.Where(x => x.Status == ExperimentLogStatus.Cancelled || x.Status == ExperimentLogStatus.Destroyed).ToList();
                    List<Domain.Entities.MethodStageDefinition> methodStages = new List<Domain.Entities.MethodStageDefinition>();
                    //List<Domain.Entities.Seedlings> seedlings = new List<Domain.Entities.Seedlings>();
                    foreach (var item in failedExperimentLog)
                    {
                        var methodStage = await methodStageRepository.FindAsync(x => x.ID.Equals(item.CurrentStageOrder), cancellationToken);
                        if(methodStage != null) 
                        {
                            var methodStageDefinition = await methodStageDefinitionRepository.FindAsync(x => x.ID.Equals(methodStage.MethodStageDefinitionId), cancellationToken);
                            if(methodStageDefinition != null)
                                methodStages.Add(methodStageDefinition);
                        }
                    }
                    //foreach (var item in g.Where(x => x.Status == ExperimentLogStatus.Cancelled || x.Status == ExperimentLogStatus.Destroyed).ToList())
                    //{
                    //    var seedling = await seedlingRepository.FindAsync(x => x.ID.Equals(item.SeedlingParentId), cancellationToken);
                    //    if(seedling != null)
                    //        seedlings.Add(seedling);
                    //}
                    return new GetSuccessCompletedExperimentLogRateDto
                    {
                        Id = g.Key.Method.ID,
                        Name = g.Key.Method?.Name ?? "Unknow",
                        Description = g.Key.Method?.Description ?? "Unknow",
                        TotalDurationDays = g.Key.Method?.MethodStages.Sum(ms => ms.DurationsDays) ?? 0,
                        CompletedExperimentLog = completedExperimentLog,
                        FailedExperimentLog = failedExperimentLog.Count(),
                        SuccessRate = completedExperimentLog + failedExperimentLog.Count() > 0 ? (int)((double)completedExperimentLog / (completedExperimentLog + failedExperimentLog.Count()) * 100) : 0,
                        MethodStages = methodStages,
                        //Seedling = seedlings,
                        //TotalExperimentLog = totalExperimentLogs
                    };
                });
            var dtoList = await Task.WhenAll(dto);
            return dtoList
                .OrderByDescending(r => r.SuccessRate)
                .ToList();
        }
    }
}
