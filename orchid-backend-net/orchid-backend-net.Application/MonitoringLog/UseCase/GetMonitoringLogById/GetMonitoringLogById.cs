using MediatR;
using orchid_backend_net.Application.MonitoringLog.Dto.MonitoringLog;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.GetMonitoringLogById
{
    public record GetMonitoringLogById(string Id) : IRequest<MonitoringLogDetailDto>;
    internal class GetMonitoringLogByIdHandler(IMonitoringLogRepository monitoringLogRepository) : IRequestHandler<GetMonitoringLogById, MonitoringLogDetailDto>
    {
        public async Task<MonitoringLogDetailDto> Handle(GetMonitoringLogById request, CancellationToken cancellationToken)
        {
            var monitoring = await monitoringLogRepository.FindProjectToAsync<MonitoringLogDetailDto>(
                queryOptions: q => q.Where(m => m.ID.Equals(request.Id)),
                cancellationToken);
            return monitoring ?? throw new NotFoundException("Không thấy monitoring log này");
        }
    }
}
