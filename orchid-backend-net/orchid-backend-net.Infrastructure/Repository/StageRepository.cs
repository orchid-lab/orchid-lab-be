using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class StageRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Stages, Stages, OrchidDbContext>(context, mapper), IStageRepository
    {
    }
}
