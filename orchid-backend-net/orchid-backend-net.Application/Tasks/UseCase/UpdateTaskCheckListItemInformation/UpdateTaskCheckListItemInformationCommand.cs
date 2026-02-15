using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.UpdateTaskCheckListItemInformation
{
    public record UpdateTaskCheckListItemInformationCommand(
        string TaskId,
        string CheckListItemId,
        string? Name,
        string? Description,
        string? ExpectedMeasureUnit,
        decimal? ExpectedMinValue,
        decimal? ExpectedMaxValue)
        : IRequest<string>;

    internal class UpdateTaskCheckListItemInformationCommandHandler 
        (ITaskRepository taskRepository,
        ICurrentUserService currentUserService): IRequestHandler<UpdateTaskCheckListItemInformationCommand, string>
    {
        public async Task<string> Handle(UpdateTaskCheckListItemInformationCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy công việc này");
            task.UpdateCheckListItem(request.CheckListItemId, request.Name, request.Description, request.ExpectedMeasureUnit, request.ExpectedMinValue, request.ExpectedMaxValue);
            task.UpdatedDate = DateTime.UtcNow;
            task.UpdatedBy = currentUserService.UserId;
            taskRepository.Update(task);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                request.CheckListItemId : 
                "Cập nhật thông tin checklist item thất bại";
        }
    }

}
