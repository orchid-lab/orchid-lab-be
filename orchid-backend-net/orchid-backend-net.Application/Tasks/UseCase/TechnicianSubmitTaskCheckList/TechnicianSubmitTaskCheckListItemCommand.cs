using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.UseCase.TechnicianSubmitTaskCheckList
{
    public record TechnicianSubmitTaskCheckListItemCommand(
        string TaskId,
        string ItemId,
        string? MeasurementUnit,
        decimal? MeasuredValue)
        : IRequest<string>;

    internal class TechnicianSubmitTaskChecklistItemCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<TechnicianSubmitTaskCheckListItemCommand, string>
    {
        public async Task<string> Handle(TechnicianSubmitTaskCheckListItemCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy công việc này");
            task.SubmitCheckListItemResult(currentUserService.UserId!, request.ItemId, request.MeasurementUnit, request.MeasuredValue);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? task.ID.ToString()
                : "Cập nhật thất bại";
        }
    }
}
