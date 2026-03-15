using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class DiseaseIncidentRepository(OrchidDbContext context, IMapper mapper)
        : RepositoryBase<DiseaseIncident, DiseaseIncident, OrchidDbContext>(context, mapper), IDiseaseIncidentRepository
    {

        private readonly OrchidDbContext _context = context;

        public async Task<DiseaseIncident?> FindWithDetailsAsync(string incidentId, CancellationToken cancellationToken)
        {
            return await _context.Set<DiseaseIncident>()
                .Include(x => x.SampleStage)
                    .ThenInclude(s => s.Samples)
                        .ThenInclude(s => s.ExperimentLog)
                .Include(x => x.Disease)
                .FirstOrDefaultAsync(x => x.ID == incidentId, cancellationToken);
        }
    }
}
