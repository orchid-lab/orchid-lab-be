using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ElementInStageRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<ElementInStage, ElementInStage, OrchidDbContext>(context, mapper), IElementInStageRepository
    {
    }
}
