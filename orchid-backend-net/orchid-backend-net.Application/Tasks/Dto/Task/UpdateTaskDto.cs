namespace orchid_backend_net.Application.Tasks.Dto.Task
{
    public class UpdateTaskDto
    {
        public required string TaskId { get; set; }
        public string? TaskAssignmentId { get; set; }
        public int? StageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
