using AutoMapper;
using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.GetReportInfor
{
    public class GetReportInforQuery : IRequest<ReportDTO>, IQuery
    {
        public string ID { get; set; }
        public GetReportInforQuery(string id)
        {
            ID = id;
        }
    }

    internal class GetReportInforQueryHandler(IMapper mapper, IReportRepository reportRepository) : IRequestHandler<GetReportInforQuery, ReportDTO>
    {
        public async Task<ReportDTO> Handle(GetReportInforQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await reportRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status != 2, cancellationToken);
                if (result == null)
                    throw new NotFoundException($"Not found report with ID :{request.ID}");
                return result.MapToReportDTO(mapper);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
