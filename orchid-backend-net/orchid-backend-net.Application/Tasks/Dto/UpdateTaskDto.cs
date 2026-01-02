namespace orchid_backend_net.Application.Tasks.Dto
{
    public class UpdateTaskDto
    {
        public required string TaskId { get; set; }
        public string? TaskAssignmentId { get; set; }
        public string? StageId { get; set; }
        public string? SampleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public bool IsForWholeExperimentLog { get; set; }
    }
}
