using orchid_backend_net.Application.ExperimentLog.Dto.Report;

namespace orchid_backend_net.Application.Common.Interfaces
{
    /// <summary>
    /// <ul>
    /// <li>Generates PDF reports for experiment process logs and experiment summaries.</li>
    /// <li>Replaces the generic object-based method with two explicit report types for clarity and type safety.</li>
    /// </ul>
    /// </summary>
    public interface IPdfReportGenerator
    {
        /// <summary>
        /// <ul>
        /// <li>Generates a process log PDF report for an experiment, including analytics, timeline, sample status, AI results, and disease incidents.</li>
        /// </ul>
        /// </summary>
        /// <param name="model">The process log report model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>PDF file as byte array.</returns>
        Task<byte[]> GenerateProcessLogAsync(
            ExperimentProcessLogReportModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// <ul>
        /// <li>Generates a summary PDF report for an experiment, suitable for submission or archival.</li>
        /// </ul>
        /// </summary>
        /// <param name="model">The summary report model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>PDF file as byte array.</returns>
        Task<byte[]> GenerateSummaryReportAsync(
            ExperimentSummaryReportModel model,
            CancellationToken cancellationToken = default);
    }
}
