namespace orchid_backend_net.API.Dto.Method
{
    /// <summary>
    /// use as data transfer object 
    /// </summary>
    /// <param name="Minvalue"></param>
    /// <param name="MaxValue"></param>
    /// <param name="ExpectedValue"></param>
    public record UpdateSampleRequirementDto(decimal? Minvalue,
        decimal? MaxValue,
        decimal? ExpectedValue);
}
