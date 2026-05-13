// Domain/IRepositories/IDiseaseRepository.cs
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IDiseaseRepository : IEFRepository<Disease, Disease>
    {
        Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    }
}