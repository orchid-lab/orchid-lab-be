namespace orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto
{
    public class CreateTaskAssignmentDto
    {
        /// <summary>
        /// depends on whether the task is a to-do task or a template task
        /// </summary>
        public required string TechnicianId { get; set; }
        /// <summary>
        /// if null, the task is for all experimentLog
        /// if not null, the task is for specific sample
        /// </summary>
        public string? SampleId { get; set; }
        public bool IsForWholeExperimentLog { get; set; }
        /// <summary>
        /// Expected end date of the task - set by the creator of the task
        /// </summary>
        public required DateTime ExpectedEndDate { get; set; }
    }
}
