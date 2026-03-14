namespace orchid_backend_net.API.Dto.DiseaseIncident
{
    /// <summary>
    /// request for review disease incident dto confirm or reject the incident, only for researcher and technician roles
    /// </summary>
    public class ReviewDiseaseIncidentRequest
    {
        /// <summary>
        /// is confirm sample is in disease or not
        /// </summary>
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// note
        /// </summary>
        public string? Note { get; set; }
    }
}
