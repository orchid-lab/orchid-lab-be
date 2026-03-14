namespace orchid_backend_net.API.Dto.DiseaseIncident
{
    /// <summary>
    /// add action for disease incident
    /// </summary>
    public class AddDiseaseIncidentActionRequest
    {
        /// <summary>
        /// description
        /// </summary>
        public string ActionDescription { get; set; } = null!;
        /// <summary>
        /// result
        /// </summary>
        public string? Result { get; set; }
    }
}
