namespace orchid_backend_net.Application.Method.Dto.Method
{
    /// <summary>
    /// Represents a failed experiment log with detailed information
    /// <ul>
    /// <li>Includes experiment log details (ID, name)</li>
    /// <li>Shows which method stage the experiment failed at (order and name)</li>
    /// <li>Contains seedling information used in the experiment</li>
    /// <li>Includes failure reason and analysis from researchers</li>
    /// </ul>
    /// </summary>
    public class FailedExperimentLogDetailDto
    {
        public required string ExperimentLogId { get; set; }
        public required string ExperimentLogName { get; set; }
        public int FailedAtStageOrder { get; set; }
        public string? FailedAtStageName { get; set; }
        public string? SeedlingLocalName { get; set; }
        public string? SeedlingScientificName { get; set; }
        public required string Status { get; set; }  // "Destroyed" or "Cancelled"
        public string? Reason { get; set; }
        public string? Issues { get; set; }
        public string? Recommendations { get; set; }
        public DateOnly? FailedDate { get; set; }
    }
}
