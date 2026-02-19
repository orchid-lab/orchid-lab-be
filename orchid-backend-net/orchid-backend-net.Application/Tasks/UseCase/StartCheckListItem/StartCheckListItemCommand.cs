using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.StartCheckListItem
{
    public record StartCheckListItemCommand(string CheckListItemId, string TaskId) : IRequest<string>;
    internal class StartCheckListItemCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService) : IRequestHandler<StartCheckListItemCommand, string>
    {
        public async Task<string> Handle(StartCheckListItemCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy công việc này.");
            task.StartChecklist(currentUserService.UserId!, request.CheckListItemId);
            taskRepository.Update(task);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? request.CheckListItemId : string.Empty;
        }
    }
}
