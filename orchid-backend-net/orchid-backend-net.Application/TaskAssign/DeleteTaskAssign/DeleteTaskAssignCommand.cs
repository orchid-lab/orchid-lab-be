using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAssign.DeleteTaskAssign
{
    public class DeleteTaskAssignCommand(string taskId) : IRequest, ICommand
    {
        public string TaskId { get; set; } = taskId;
    }

    internal class DeleteTaskAssignCommandHandler(ITaskAssignRepository taskAssignRepository) : IRequestHandler<DeleteTaskAssignCommand>
    {
        public async Task Handle(DeleteTaskAssignCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tasksAssigns = await taskAssignRepository.FindAllAsync(x => x.ID.Equals(request.TaskId), cancellationToken);
                foreach(var assign in tasksAssigns)
                {
                    assign.Status = false;
                    taskAssignRepository.Update(assign);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
