namespace orchid_backend_net.Application.Tasks.Dto
{
    public class CreateTaskDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// depends on whether the task is a to-do task or a template task
        /// </summary>
        public string? TechnicianId { get; set; }
        /// <summary>
        /// if stage id is null => the task is a to-do task
        /// if stage is not null => the task is a template task
        /// </summary>
        public string? StageId { get; set; }
        /// <summary>
        /// if null, the task is for all experimentLog
        /// if not null, the task is for specific sample
        /// </summary>
        public string? SampleId { get; set; }
        public bool IsForWholeExperimentLog { get; set; }
        /// <summary>
        /// Expected end date of the task - set by the creator of the task
        /// </summary>
        public DateTime ExpectedEndDate { get; set; }
    }
}
