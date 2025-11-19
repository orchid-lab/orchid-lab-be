using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand(string taskId, int status) : IRequest<string>
    {
        public string TaskId { get; set; } = taskId;
        public int Status { get; set; } = status;
    }
    internal class UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService) : IRequestHandler<UpdateTaskStatusCommand, string>
    {
        public async Task<string> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(x => x.ID == request.TaskId, cancellationToken);
            task.Status = request.Status;
            task.Update_date = DateTime.UtcNow;
            task.Update_by = currentUserService.UserId;
            taskRepository.Update(task);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 
                ? $"Updated task ID :{request.TaskId} with status {request.Status}" 
                : $"Failed to update task with ID :{request.TaskId}";
        }
    }
}
