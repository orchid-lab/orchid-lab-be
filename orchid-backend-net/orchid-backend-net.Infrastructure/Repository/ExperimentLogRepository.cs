using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ExperimentLogRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<ExperimentLogs, ExperimentLogs, OrchidDbContext>(dbContext, mapper), IExperimentLogRepository
    {
    }
}
