using AutoMapper;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ConfigRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Domain.Entities.Config, Domain.Entities.Config, OrchidDbContext>(dbContext, mapper), Domain.IRepositories.IConfigRepository
    {
    }
}
