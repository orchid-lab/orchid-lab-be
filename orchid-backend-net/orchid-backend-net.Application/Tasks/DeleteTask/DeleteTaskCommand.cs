using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.TaskAssign.DeleteTaskAssign;
using orchid_backend_net.Application.TaskAttribute.DeleteTaskAttribute;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Tasks.DeleteTask
{
    public class DeleteTaskCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public DeleteTaskCommand() { }
        public DeleteTaskCommand(string ID)
        {
            this.ID = ID;
        }

    }

    internal class DeleteTaskCommandHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService, ISender sender) : IRequestHandler<DeleteTaskCommand, string>
    {
        public async Task<string> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await taskRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                task.Delete_date = DateTime.UtcNow;
                task.Delete_by = currentUserService.UserId;
                task.Status = 5;
                await sender.Send(new DeleteTaskAssignCommand(task.ID), cancellationToken);
                await sender.Send(new DeleteTaskAttributeCommand(task.ID), cancellationToken);
                return await taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Task {task.Name} is deleted." : $"Failed to delete task {task.Name}";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
