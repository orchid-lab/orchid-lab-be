using MediatR;
using orchid_backend_net.Application.Seedling.Dto;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Seedling.UseCase.GetHybridSuccessRate
{
    /// <summary>
    /// <ul>
    /// <li>So sánh tỷ lệ thành công giữa các cây giống bố mẹ và phương pháp lai.</li>
    /// <li>"Thành công" = ExperimentLog có Status = Completed và tỷ lệ sống >= threshold.</li>
    /// </ul>
    /// </summary>
    public record GetHybridSuccessRateQuery(
        string? SeedlingParentId,
        int? MethodId,
        DateOnly? FromDate,
        DateOnly? ToDate
    ) : IRequest<List<HybridSuccessRateDto>>;

    internal class GetHybridSuccessRateQueryHandler(
        IExperimentLogRepository experimentLogRepository
    ) : IRequestHandler<GetHybridSuccessRateQuery, List<HybridSuccessRateDto>>
    {
        public async Task<List<HybridSuccessRateDto>> Handle(
            GetHybridSuccessRateQuery request,
            CancellationToken cancellationToken)
        {
            var logs = await experimentLogRepository.FindAllAsync(
                el =>
                    (request.SeedlingParentId == null || el.SeedlingParentId == request.SeedlingParentId) &&
                    (request.MethodId == null || el.MethodId == request.MethodId) &&
                    (request.FromDate == null || el.StartDate >= request.FromDate) &&
                    (request.ToDate == null || el.EndDate <= request.ToDate),
                cancellationToken);

            return logs
                .GroupBy(el => new { el.SeedlingParentId, el.MethodId })
                .Select(g =>
                {
                    var total = g.Count();
                    var completed = g.Count(el => el.Status == ExperimentLogStatus.Completed);
                    var avgSurvival = g
                        .Where(el => el.Samples.Count > 0)
                        .Select(el =>
                            (double)el.Samples.Count(s => !s.ExecutionDate.HasValue) / el.Samples.Count * 100)
                        .DefaultIfEmpty(0)
                        .Average();

                    return new HybridSuccessRateDto
                    {
                        SeedlingParentId = g.Key.SeedlingParentId,
                        SeedlingParentName = g.First().SeedlingParent?.LocalName ?? "Unknown",
                        MethodId = g.Key.MethodId,
                        MethodName = g.First().Method?.Name ?? "Unknown",
                        TotalExperiments = total,
                        CompletedExperiments = completed,
                        SuccessRate = total > 0
                            ? Math.Round((double)completed / total * 100, 1) : 0,
                        AverageSurvivalRate = Math.Round(avgSurvival, 1)
                    };
                })
                .OrderByDescending(r => r.SuccessRate)
                .ToList();
        }
    }
}
