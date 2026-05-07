using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem;

namespace orchid_backend_net.API.Dto.Task
{
    /// <summary>
    /// request dto for create task command
    /// </summary>
    public class CreateTaskRequestDto
    {
        /// <summary>
        /// parameter for passing thru the required properties for creating a task, such as name, description, stageId, etc.
        /// </summary>
        public required CreateTaskDto Parameter { get; set; }
        /// <summary>
        /// task attribute like the command
        /// </summary>
        public List<CreateTaskAttributeDto>? CreateTaskAttributes { get; set; }
        /// <summary>
        /// this task is assign to who, and when is the expected end date, etc. - same as the command
        /// </summary>
        public CreateTaskAssignmentDto? CreateTaskAssignment { get; set; }
        /// <summary>
        /// check list for task
        /// </summary>
        public List<CreateTaskCheckListItemDto>? CreateTaskCheckListItems { get; set; }
    }
}
