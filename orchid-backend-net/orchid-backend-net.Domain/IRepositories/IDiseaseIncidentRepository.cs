using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IDiseaseIncidentRepository : IEFRepository<DiseaseIncident, DiseaseIncident>
    {
        Task<DiseaseIncident?> FindWithDetailsAsync(
            string incidentId,
            CancellationToken cancellationToken);
    }
}
