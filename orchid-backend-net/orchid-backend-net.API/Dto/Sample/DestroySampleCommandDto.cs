namespace orchid_backend_net.API.Dto.Sample
{
    /// <summary>
    /// dto binding for sample DELETE api
    /// </summary>
    public class DestroySampleCommandDto
    {
        /// <summary>
        /// reason of sample destroy
        /// </summary>
        public string? Reason { get; set; }
    }
}
