using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using System.Text.Json.Serialization;

namespace orchid_backend_net.API.Dto.Task
{
    /// <summary>
    /// request payload for update task (matches frontend payload)
    /// </summary>
    public class UpdateTaskRequestDto
    {
        public string? TaskId { get; set; }
        public int? StageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [JsonPropertyName("createTaskAttribute")]
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; }

        [JsonPropertyName("updateTaskAttribute")]
        public List<UpdateTaskAttributeDto>? UpdateTaskAttribute { get; set; }

        public UpdateTaskAssignmentDto? UpdateTaskAssignment { get; set; }
    }
}
