using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Domain.IRepositories
{
    public interface IMonitoringLogRepository : IEFRepository<MonitoringLogs, MonitoringLogs>
    {
        /// <summary>
        /// Finds monitoring log by ID with navigation to SampleStage > Samples > ExperimentLog.
        /// Used to get researcher ID for approval workflow.
        /// </summary>
        Task<MonitoringLogs?> FindByIdWithResearcherAsync(string id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Finds monitoring log by ID with LogDetails collection.
        /// Used for updating log details.
        /// </summary>
        Task<MonitoringLogs?> FindByIdWithLogDetailsAsync(string id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Finds the latest approved monitoring log for a sample stage with its log details.
        /// </summary>
        Task<MonitoringLogs?> FindLatestApprovedLogWithDetailsBySampleStageIdAsync(
            string sampleStageId, 
            CancellationToken cancellationToken = default);
    }
}
