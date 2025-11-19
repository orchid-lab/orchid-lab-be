using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Report
{
    public static class ReportMappingExtentations
    {
        public static ReportDTO MapToReportDTO(this Reports report, IMapper mapper)
            => mapper.Map<ReportDTO>(report);
        public static List<ReportDTO> MapToReprotDTOList(this IEnumerable<Reports> reportList, IMapper mapper)
            => reportList.Select(x => x.MapToReportDTO(mapper)).ToList();
    }
}
