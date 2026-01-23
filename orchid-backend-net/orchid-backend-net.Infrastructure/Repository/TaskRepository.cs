using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class TaskRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<Tasks, Tasks, OrchidDbContext>(dbContext, mapper), ITaskRepository
    {
        public async Task<List<Tasks>> GetAllTaskTemplateByStageId(int stageId, CancellationToken cancellationToken)
         => await FindAllAsync(t => t.StageId == stageId
                && t.Status == Domain.Common.Enum.TaskStatus.Template, cancellationToken);

        public async Task<Tasks?> GetTemplateForConversionAsync(string templateId, CancellationToken cancellationToken)
        {
            return await dbContext
                 .Tasks.Include(t => t.TaskAttributes)
                 .SingleOrDefaultAsync(t => t.ID == templateId, cancellationToken);
        }
    }
}
