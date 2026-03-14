namespace orchid_backend_net.API.Dto.ExperimentLog
{
    /// <summary>
    /// update experiment log status dto
    /// </summary>
    /// <param name="Status">status string follow up with experiment log status enum</param>
    /// <param name="BatchId">batch id if user change experiment log stage into different batch require</param>
    /// <param name="Reason"></param>
    /// <param name="Conclusion"></param>
    /// <param name="Issues"></param>
    /// <param name="Recommendations"></param>
    public record UpdateExperimentLogStatusDto(
        string Status,
        int? BatchId,
        string? Reason,
        string? Conclusion,
        string? Issues,
        string? Recommendations);
}
