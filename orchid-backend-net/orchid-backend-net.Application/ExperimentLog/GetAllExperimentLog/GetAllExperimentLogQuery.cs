using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.GetAllExperimentLog
{
    public class GetAllExperimentLogQuery : IRequest<PageResult<ExperimentLogDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Filter { get; set; }
        public string? SearchTerm { get; set; }
        public GetAllExperimentLogQuery(int pageNumber, int pageSize, string filter, string searchTerm)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Filter = filter;
            this.SearchTerm = searchTerm;
        }
        public GetAllExperimentLogQuery() { }

    }

    internal class GetAllExperimentLogQueryHandler(IExperimentLogRepository experimentLogRepository) : IRequestHandler<GetAllExperimentLogQuery, PageResult<ExperimentLogDTO>>
    {

        public async Task<PageResult<ExperimentLogDTO>> Handle(GetAllExperimentLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Domain.Entities.ExperimentLogs> queryOptions(IQueryable<Domain.Entities.ExperimentLogs> query)
                {
                    if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                        query = query.Where(x => x.TissueCultureBatch.Name.Contains(request.SearchTerm));

                    if (!string.IsNullOrWhiteSpace(request.Filter))
                        query = query.Where(x => x.MethodID == request.Filter);

                    return query;
                }

                var experimentLogs = await experimentLogRepository.FindAllProjectToAsync<ExperimentLogDTO>(
                    pageNo: request.PageNumber,
                    pageSize: request.PageSize,
                    queryOptions: queryOptions,
                    cancellationToken: cancellationToken);

                return experimentLogs.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
