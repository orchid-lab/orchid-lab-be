using AutoMapper;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog
{
    public static class ExperimentLogMappingExtentations
    {
        public static ExperimentLogDTO MapToExperimentLogDTO(this ExperimentLogs experimentLog, IMapper mapper)
            => mapper.Map<ExperimentLogDTO>(experimentLog);
        public static List<ExperimentLogDTO> MapToExperimentLogDTOList(this IEnumerable<ExperimentLogs> experimentLogList, IMapper mapper)
            => experimentLogList.Select(x => x.MapToExperimentLogDTO(mapper)).ToList();
    }
}
