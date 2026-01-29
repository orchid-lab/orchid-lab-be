using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface ISampleStageRepository
    {
        Task<SampleStage> FindSampleStageById(string id, CancellationToken cancellationToken);
    }
}
