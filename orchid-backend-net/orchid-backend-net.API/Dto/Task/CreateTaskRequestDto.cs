using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem;
using System.Text.Json.Serialization;

namespace orchid_backend_net.API.Dto.Task
{
    /// <summary>
    /// request dto for create task command (matches frontend payload)
    /// </summary>
    public class CreateTaskRequestDto
    {
        /// <summary>
        /// backward-compatible flat field for task name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// backward-compatible flat field for description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// backward-compatible flat field for stage id
        /// </summary>
        public int? StageId { get; set; }

        /// <summary>
        /// legacy frontend field: createTaskAttribute
        /// </summary>
        [JsonPropertyName("createTaskAttribute")]
        public List<CreateTaskAttributeDto>? CreateTaskAttribute { get; set; }

        /// <summary>
        /// this task is assign to who, and when is the expected end date, etc. - same as the command
        /// </summary>
        public CreateTaskAssignmentDto? CreateTaskAssignment { get; set; }

        /// <summary>
        /// legacy frontend field: createTaskCheckListItemDtos
        /// </summary>
        [JsonPropertyName("createTaskCheckListItemDtos")]
        public List<CreateTaskCheckListItemDto>? CreateTaskCheckListItemDtos { get; set; }
    }
}
