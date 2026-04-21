namespace orchid_backend_net.Domain.Common.Enum
{
    /// <summary>
    /// Status of monitoring log throughout its lifecycle.
    /// <ul>
    /// <li>Created: Technician just created, not yet submitted</li>
    /// <li>WaitingForApproval: First submission, waiting for researcher review</li>
    /// <li>Approved: Researcher approved the report</li>
    /// <li>Rejected: Researcher rejected, needs revision</li>
    /// <li>Revised: Technician revised and resubmitted after rejection</li>
    /// </ul>
    /// </summary>
    public enum MonitoringLogStatus
    {
        Created = 0,
        WaitingForApproval = 1,
        Approved = 2,
        Rejected = 3,
        Revised = 4
    }
}
