using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IExperimentLogRepository : IEFRepository<ExperimentLogs, ExperimentLogs>
    {
        Task<ExperimentLogs> GetExperimentLogByIdAsync(string id, CancellationToken cancellationToken);
        Task<ExperimentLogs?> GetForReportAsync(string id, CancellationToken cancellationToken);
    }
}
