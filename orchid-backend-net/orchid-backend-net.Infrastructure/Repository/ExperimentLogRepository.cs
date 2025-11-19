using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ExperimentLogRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<ExperimentLogs, ExperimentLogs, OrchidDbContext>(context, mapper), IExperimentLogRepository
    {
    }
}
