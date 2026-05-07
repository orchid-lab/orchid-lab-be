using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;

namespace orchid_backend_net.API.Dto.Task
{
    /// <summary>
    /// parameter for update task, including the basic information of the task, the attributes of the task, and the assignment of the task
    /// </summary>
    public class UpdateTaskRequestDto
    {
        /// <summary>
        /// param
        /// </summary>
        public required UpdateTaskDto Parameter { get; set; }
        /// <summary>
        /// create task attribute - for the new attribute that need to be added to the task
        /// </summary>
        public List<CreateTaskAttributeDto>? CreateTaskAttributes { get; set; }
        /// <summary>
        /// update the old one - for the existing attribute that need to be updated
        /// </summary>
        public List<UpdateTaskAttributeDto>? UpdateTaskAttributes { get; set; }
        /// <summary>
        /// change technician or else
        /// </summary>
        public UpdateTaskAssignmentDto? UpdateTaskAssignment { get; set; }
    }
}
