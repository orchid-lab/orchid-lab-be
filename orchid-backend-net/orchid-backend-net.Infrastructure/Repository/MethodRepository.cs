using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class MethodRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Methods, Methods, OrchidDbContext>(context, mapper), IMethodRepository
    {
    }
}
