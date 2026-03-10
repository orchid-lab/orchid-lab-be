using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class MonitoringLogRepository(OrchidDbContext context, IMapper mapper) 
        : RepositoryBase<Domain.Entities.MonitoringLogs, Domain.Entities.MonitoringLogs, OrchidDbContext>(context, mapper), 
          Domain.IRepositories.IMonitoringLogRepository
    {
        private readonly OrchidDbContext _context = context;

        public async Task<MonitoringLogs?> FindByIdWithResearcherAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<MonitoringLogs>()
                .Include(m => m.SampleStage)
                    .ThenInclude(s => s.Samples)
                        .ThenInclude(sa => sa.ExperimentLog)
                .FirstOrDefaultAsync(m => m.ID == id, cancellationToken);
        }

        public async Task<MonitoringLogs?> FindByIdWithLogDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<MonitoringLogs>()
                .Include(m => m.LogDetails)
                .FirstOrDefaultAsync(m => m.ID == id, cancellationToken);
        }

        public async Task<MonitoringLogs?> FindLatestApprovedLogWithDetailsBySampleStageIdAsync(
            string sampleStageId, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<MonitoringLogs>()
                .Include(m => m.LogDetails)
                .Where(m => m.SampleStageId == sampleStageId
                            && m.Status == MonitoringLogStatus.Approved
                            && m.IsNewest)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
