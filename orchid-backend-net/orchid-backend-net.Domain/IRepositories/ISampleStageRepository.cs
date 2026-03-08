using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface ISampleStageRepository : IEFRepository<SampleStage, SampleStage>
    {
        Task<SampleStage> FindSampleStageById(string id, CancellationToken cancellationToken);
    }
}
