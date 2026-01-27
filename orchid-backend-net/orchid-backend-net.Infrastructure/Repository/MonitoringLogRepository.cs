using AutoMapper;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class MonitoringLogRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Domain.Entities.MonitoringLogs, Domain.Entities.MonitoringLogs, OrchidDbContext>(context, mapper), Domain.IRepositories.IMonitoringLogRepository
    {
    }
}
