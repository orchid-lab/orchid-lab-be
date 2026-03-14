using AutoMapper;
using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class ExperimentLogRepository(OrchidDbContext dbContext, IMapper mapper) : RepositoryBase<ExperimentLogs, ExperimentLogs, OrchidDbContext>(dbContext, mapper), IExperimentLogRepository
    {
        public async Task<ExperimentLogs> GetExperimentLogByIdAsync(string id, CancellationToken cancellationToken)
        => await this.FindAsync(el => el.ID == id, cancellationToken)
                ?? throw new NotFoundException("Không thấy thí nghiệm này");
        public async Task<ExperimentLogs?> GetForReportAsync(string id, CancellationToken cancellationToken)
        => await FindAsync(
            queryOptions: q => q
                .Where(el => el.ID == id)
                    .Include(el => el.SeedlingParent)
                    .Include(el => el.Method)
                        .ThenInclude(m => m.MethodStages)
                            .ThenInclude(ms => ms.MethodStageDefinition)
                    .Include(el => el.Samples)
                        .ThenInclude(s => s.SampleStages)
                            .ThenInclude(ss => ss.SampleStageDefinition)
                    .Include(el => el.Samples)
                        .ThenInclude(s => s.SampleStages)
                            .ThenInclude(ss => ss.MonitoringLogs)
                                .ThenInclude(ml => ml.AnalyticResult)
                    .Include(el => el.Samples)
                        .ThenInclude(s => s.SampleStages)
                            .ThenInclude(ss => ss.MonitoringLogs)
                                .ThenInclude(ml => ml.Disease),
            cancellationToken);
    }
}
