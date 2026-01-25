namespace orchid_backend_net.API.Dto.Sample
{
    /// <summary>
    /// this dto is use to bypass data into sample use case command
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="Notes"></param>
    public record UpdateSampleInformationDto(string? Name, string? Description, string? Notes);
}
