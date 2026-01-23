using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface ITaskRepository : IEFRepository<Tasks, Tasks>
    {
        Task<Tasks?> GetTemplateForConversionAsync(string templateId, CancellationToken cancellationToken);

        Task<List<Tasks>> GetAllTaskTemplateByStageId(int stageId, CancellationToken cancellationToken);
    }
}
