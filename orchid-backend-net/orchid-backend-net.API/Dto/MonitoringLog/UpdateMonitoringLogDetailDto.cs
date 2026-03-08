using orchid_backend_net.Application.MonitoringLog.UseCase.UpdateMonitoringLogDetail;

namespace orchid_backend_net.API.Dto.MonitoringLog
{
    /// <summary>
    /// this record is represent the data transfer object for updating monitoring log details after rejection.
    /// </summary>
    /// <param name="UpdatedLogDetails"></param>
    public record UpdateMonitoringLogDetailDto(List<UpdateLogDetailDto> UpdatedLogDetails);

}
