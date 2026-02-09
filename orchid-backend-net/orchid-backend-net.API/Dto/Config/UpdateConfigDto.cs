namespace orchid_backend_net.API.Dto.Config
{
    /// <summary>
    /// update config dto
    /// </summary>
    /// <param name="ConfigName"></param>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    public record UpdateConfigDto(string? ConfigName, string? Key, decimal? Value);
}
