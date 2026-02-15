using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.RemoveTaskCheckListItem
{
    public record RemoveTaskCheckListItemCommand(string TaskId, string CheckListItemId) : IRequest<string>;
    internal class RemoveTaskCheckListItemCommandHandler
        (ITaskRepository taskRepository, 
        ICurrentUserService currentUserService): IRequestHandler<RemoveTaskCheckListItemCommand, string>
    {
        public async Task<string> Handle(RemoveTaskCheckListItemCommand request, CancellationToken cancellationToken)
        {
            var tasks = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy công việc này");

            tasks.RemoveCheckListItem(request.CheckListItemId);
            tasks.UpdatedDate = DateTime.UtcNow;
            tasks.UpdatedBy = currentUserService.UserId;
            taskRepository.Update(tasks);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 
                ? request.CheckListItemId 
                : "Xóa checklist item thất bại"; 
        }
    }
}
