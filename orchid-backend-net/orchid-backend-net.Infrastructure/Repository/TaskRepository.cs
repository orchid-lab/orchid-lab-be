using AutoMapper;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class TaskRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<Domain.Entities.Tasks, Domain.Entities.Tasks, OrchidDbContext>(context, mapper), ITaskRepository
    {
    }
}
