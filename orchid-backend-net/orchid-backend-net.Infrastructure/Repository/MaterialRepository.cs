using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class MaterialRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Materials, Materials, OrchidDbContext>(context, mapper), IMaterialRepository
    {
    }
}
