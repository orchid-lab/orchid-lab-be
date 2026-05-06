using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.ExperimentLog.Dto.ExperimentLog;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UseCase.GetAllExperimentLog
{
    public record GetAllExperimentLogQuery(
        int PageNo, 
        int PageSize, 
        string? NameSearchTerm, 
        string? MethodNameSearchTerm, 
        string? ResearcherId,
        string? TechnicianId,
        int? CurrentStageOrder) : IRequest<PageResult<ExperimentLogDto>>;
    internal class GetAllExperimentLogQueryHandler(
        IExperimentLogRepository experimentLogRepository) : IRequestHandler<GetAllExperimentLogQuery, PageResult<ExperimentLogDto>>
    {
        public async Task<PageResult<ExperimentLogDto>> Handle(GetAllExperimentLogQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ExperimentLogs> queryOptions(IQueryable<ExperimentLogs> query) 
            {
                if(!string.IsNullOrWhiteSpace(request.NameSearchTerm))
                {
                    query = query.Where(el => el.Name.ToLower().Contains(request.NameSearchTerm.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(request.MethodNameSearchTerm))
                {
                    query = query.Where(el => el.Method.Name.ToLower().Contains(request.MethodNameSearchTerm.ToLower()));
                }
                if(request.CurrentStageOrder is not null)
                {
                    query = query.Where(el => el.CurrentStageOrder ==  request.CurrentStageOrder);
                }
                if(!string.IsNullOrWhiteSpace(request.ResearcherId))
                {
                    query = query.Where(el => el.CreatedBy.Equals(request.ResearcherId));
                }
                if(!string.IsNullOrWhiteSpace(request.TechnicianId))
                {
                    query = query.Where(el => el.AssignedTo.Equals(request.TechnicianId));
                }
                return query.OrderBy(x => x.CreatedDate);
            }

            var el = await experimentLogRepository.FindAllProjectToAsync<ExperimentLogDto>(
                pageNo: request.PageNo,
                pageSize: request.PageSize,
                queryOptions: queryOptions,
                cancellationToken);
            return el.ToAppPageResult();
        }
    }
}
