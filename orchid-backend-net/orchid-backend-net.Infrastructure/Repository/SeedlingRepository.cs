using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class SeedlingRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Seedlings, Seedlings, OrchidDbContext>(context, mapper), ISeedlingRepository
    {
    }
}
