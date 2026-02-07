using AutoMapper;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class SafeProcedureRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Domain.Entities.SafeProcedure, Domain.Entities.SafeProcedure, OrchidDbContext>(dbContext, mapper), Domain.IRepositories.ISafeProcedureRepository
    {
    }
}
