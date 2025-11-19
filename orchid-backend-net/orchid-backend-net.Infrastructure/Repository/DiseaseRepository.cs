using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class DiseaseRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Diseases, Diseases, OrchidDbContext>(context, mapper), IDiseaseRepository
    {
    }
}
