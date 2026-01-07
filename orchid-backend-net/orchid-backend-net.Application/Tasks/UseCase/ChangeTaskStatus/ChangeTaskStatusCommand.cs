using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Tasks.Helper;
using orchid_backend_net.Application.Tasks.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.ChangeTaskStatus
{
    /// <summary>
    /// use for technician to update status 
    /// and researcher approve task to completed
    /// </summary>
    public class ChangeTaskStatusCommand : IRequest<string>
    {
        public required string TodoTaskId { get; set; }
        public required string Status { get; set; }
        public string? TaskAssignmentId { get; set; }
        /// <summary>
        /// only has value when status is waiting for approval
        /// </summary>
        public DateTime? EndDate { get; set; }
    }

    internal class ChangeTaskStatusCommandHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider) : IRequestHandler<ChangeTaskStatusCommand, string>
    {
        public async Task<string> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(t => t.ID == request.TodoTaskId, cancellationToken);

            if (task == null)
                throw new NotFoundException("Không tìm thấy task này.");

            var parsedStatus = TaskPolicy.ValidateTaskStatusChange(task, request, dateTimeProvider);

            //information
            task.UpdatedDate = DateTime.UtcNow;
            task.UpdatedBy = currentUserService.UserId;

            //event trigger
            TaskStatusActionDispatcher.Dispatch(task, parsedStatus, currentUserService.UserId!, request.EndDate);

            //task assignment
            if (TaskPolicy.IsCompletedStatus(parsedStatus) && request.EndDate != null)
            {   
                task.TaskAssignment.EndDate = request.EndDate;
            }

            taskRepository.Update(task);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Cập nhật trạng thái của task thành công" :
                "Cập nhật trạng thái của task thất bại";
        }
    }
}
