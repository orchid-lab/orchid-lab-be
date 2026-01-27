using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.GetAllMonitoring
{
    public record GetAllMonitoringLogQuery(int PageNo, int PageSize, string? TechnicianId, string? SampleName, string? NameSearchTerm) : IRequest<PageResult<MonitoringLogDto>>;
    internal class GetAllMonitoringLogQueryHandler(IMonitoringLogRepository monitoringLogRepository) : IRequestHandler<GetAllMonitoringLogQuery, PageResult<MonitoringLogDto>>
    {
        public async Task<PageResult<MonitoringLogDto>> Handle(GetAllMonitoringLogQuery request, CancellationToken cancellationToken)
        {
            IQueryable<MonitoringLogs> queryOptions(IQueryable<MonitoringLogs> query)
            {
                if (!string.IsNullOrEmpty(request.TechnicianId))
                {
                    query = query.Where(ml => ml.UserId == request.TechnicianId);
                }
                if (!string.IsNullOrEmpty(request.SampleName))
                {
                    query = query.Where(ml => ml.SampleStage.Samples.Name.Contains(request.SampleName));
                }
                if(!string.IsNullOrEmpty(request.NameSearchTerm))
                {
                    query = query.Where(ml => ml.Name.Contains(request.NameSearchTerm));
                }
                return query;
            }

            var result = await monitoringLogRepository.FindAllProjectToAsync<MonitoringLogDto>(
                 request.PageNo,
                 request.PageSize,
                 queryOptions,
                 cancellationToken
            );
            return result.ToAppPageResult();
        }
    }
}
