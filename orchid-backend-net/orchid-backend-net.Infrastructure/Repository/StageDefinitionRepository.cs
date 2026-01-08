using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class StageDefinitionRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<StageDefinition, StageDefinition, OrchidDbContext>(dbContext, mapper), IStageDefinitionRepository
    {
    }
}
