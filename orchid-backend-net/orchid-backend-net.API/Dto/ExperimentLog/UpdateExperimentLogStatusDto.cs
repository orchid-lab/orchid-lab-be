namespace orchid_backend_net.API.Dto.ExperimentLog
{
    /// <summary>
    /// update experiment log status dto
    /// </summary>
    /// <param name="Status">status string follow up with experiment log status enum</param>
    /// <param name="BatchId">batch id if user change experiment log stage into different batch require</param>
    public record UpdateExperimentLogStatusDto(
        string Status,
        int? BatchId);
}
