using MediatR;
using orchid_backend_net.Application.Method.Dto.Method;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.GetFailedExperimentLogDetails
{
    /// <summary>
    /// Query to retrieve failed experiment log details for a specific method with pagination
    /// <ul>
    /// <li>Returns detailed information about failed experiments (Destroyed or Cancelled)</li>
    /// <li>Includes method stage where experiment failed and seedling information</li>
    /// <li>Supports pagination with skip and take parameters</li>
    /// <li>Results ordered by failure date (newest first)</li>
    /// </ul>
    /// </summary>
    public class GetFailedExperimentLogDetailsQuery : IRequest<PagedFailedExperimentLogResult>
    {
        public int MethodId { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;

        public GetFailedExperimentLogDetailsQuery(int methodId, int skip = 0, int take = 10)
        {
            MethodId = methodId;
            Skip = skip;
            Take = take;
        }
    }

    /// <summary>
    /// Represents paginated result of failed experiment logs
    /// </summary>
    public class PagedFailedExperimentLogResult
    {
        public int TotalCount { get; set; }
        public List<FailedExperimentLogDetailDto> Items { get; set; } = new();
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
