namespace orchid_backend_net.API.Dto.Task
{
    /// <summary>
    /// This dto using for technician to submit value through api
    /// </summary>
    public class SubmitTaskChecklistItemDto
    {
        /// <summary>
        /// measurement Unit
        /// </summary>
        public string? MeasurementUnit { get; set; }
        /// <summary>
        /// measurement value
        /// </summary>
        public decimal? MeasuredValue { get; set; }
    }
}
