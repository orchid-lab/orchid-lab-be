using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IBatchesRepository : IEFRepository<Batches, Batches>
    {
        Task<Batches> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
