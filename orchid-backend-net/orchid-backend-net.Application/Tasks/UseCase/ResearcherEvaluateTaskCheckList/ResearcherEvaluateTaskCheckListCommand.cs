using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orchid_backend_net.Application.Tasks.UseCase.ResearcherEvaluateTaskCheckList
{
    public record ResearcherEvaluateTaskCheckListCommand(
        string TaskId, 
        string ItemId,
        bool IsPass) : IRequest<string>;

    internal class ResearcherEvaluateTaskCheckListCommandHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<ResearcherEvaluateTaskCheckListCommand, string>
    {
        public async Task<string> Handle(ResearcherEvaluateTaskCheckListCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.FindAsync(t => t.ID.Equals(request.TaskId), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy công việc này");
            task.EvaluateCheckListItem(currentUserService.UserId!, request.ItemId, request.IsPass);
            return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? task.ID.ToString()
                : "Cập nhật thất bại";
        }
    }
}
