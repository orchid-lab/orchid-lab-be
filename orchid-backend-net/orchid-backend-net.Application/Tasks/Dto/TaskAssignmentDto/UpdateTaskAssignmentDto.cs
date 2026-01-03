namespace orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto
{
    public class UpdateTaskAssignmentDto
    {
        public required string TaskAssignmentId { get; set; } 
        public string? SampleId { get; set; }
        public bool IsForWholeExperimentLog { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
    }
}
