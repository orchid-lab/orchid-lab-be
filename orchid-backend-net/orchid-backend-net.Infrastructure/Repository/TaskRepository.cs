using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class TaskRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Tasks, Tasks, OrchidDbContext>(dbContext, mapper), ITaskRepository
    {
    }
}
