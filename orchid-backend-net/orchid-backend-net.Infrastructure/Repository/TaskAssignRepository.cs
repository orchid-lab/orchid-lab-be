using AutoMapper;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class TaskAssignRepository(OrchidDbContext context, IMapper mapper) : RepositoryBase<TasksAssign, TasksAssign, OrchidDbContext>(context, mapper), ITaskAssignRepository
    {
    }
}
