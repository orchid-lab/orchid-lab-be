namespace orchid_backend_net.API.Dto.ExperimentLog
{
    /// <summary>
    /// use this dto only for update experiment log information
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Notes"></param>
    /// <param name="ExpectedSampleCount"></param>
    /// <param name="Objective"></param>
    public record UpdateExperimentLogInformationDto(
        string? Name,
        string? Notes,
        int? ExpectedSampleCount,
        string? Objective);
}
