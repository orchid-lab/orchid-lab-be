using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto
{
    public class UpdateTaskAssignmentDto
    {
        public required string TaskAssignmentId { get; set; }
        public TaskTargetType? TargetType { get; set; }
        public string? TargetId { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
    }
}
