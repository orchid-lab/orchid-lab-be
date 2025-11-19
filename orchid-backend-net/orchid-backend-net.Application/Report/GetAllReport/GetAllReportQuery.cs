using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Extension;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Common.Pagination;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.GetAllReport
{
    public class GetAllReportQuery : IRequest<PageResult<ReportDTO>>, IQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? TechnicianId { get; set; }
        public string? ExperimentLogId { get; set; }
        public string? StageId { get; set; }
        public GetAllReportQuery() { }
        public GetAllReportQuery(int pageNumber, int pageSize, string? technicianId, string? experimentLogId, string? stageId)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TechnicianId = technicianId;
            ExperimentLogId = experimentLogId;
            StageId = stageId;
        }
    }

    internal class GetAllReportQueryHandler(IReportRepository reportRepository, IMapper mapper) : IRequestHandler<GetAllReportQuery, PageResult<ReportDTO>>
    {

        public async Task<PageResult<ReportDTO>> Handle(GetAllReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Reports> queryOptions(IQueryable<Reports> query)
                {
                    if(!string.IsNullOrWhiteSpace(request.TechnicianId))
                        query = query.Where(x => x.TechnicianID.ToLower().Equals(request.TechnicianId.ToLower()));
                    if (!string.IsNullOrWhiteSpace(request.ExperimentLogId))
                        query = query.Where(report => report.Sample.Linkeds.Any(linkeds => linkeds.ExperimentLogID.Equals(request.ExperimentLogId)));
                    if(!string.IsNullOrEmpty(request.StageId))
                        query = query.Where(x => x.StageId.Equals(request.StageId));
                    return query;
                }

                var reports = await reportRepository.FindAllProjectToAsync<ReportDTO>(
                    pageNo: request.PageNumber,
                    pageSize: request.PageSize,
                    queryOptions: queryOptions,
                    cancellationToken: cancellationToken);
                return reports.ToAppPageResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
