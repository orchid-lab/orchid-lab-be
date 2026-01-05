using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.DeleteTask
{
    public class DeleteTaskCommand(string taskId) : IRequest<string>
    {
        public required string TaskId { get; set; } = taskId;
    }

    internal class DeleteTaskCommandHandler(
        ITaskRepository taskRepository, 
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IRequestHandler<DeleteTaskCommand, string>
    {
        public async Task<string> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken);
            if (task is null)
            {
                throw new NotFoundException("Không tìm thấy task.");
            }
            TaskPolicy.ValidateTaskWorkingHour(null ,dateTimeProvider);
            task.Status = Domain.Common.Enum.TaskStatus.Deleted;
            task.DeletedDate = DateTime.UtcNow;
            task.DeletedBy = currentUserService.UserId;
            taskRepository.Update(task);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Xóa thành công." : "Xóa thất bại.";
        }
    }
}
