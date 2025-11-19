using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskAttribute.DeleteTaskAttribute
{
    public class DeleteTaskAttributeCommand(string taskId) : IRequest, ICommand
    {
        public string TaskId { get; set; } = taskId;
    }

    internal class DeleteTaskAttributeCommandHandler(ITaskAttributeRepository taskAttributeRepository) : IRequestHandler<DeleteTaskAttributeCommand>
    {
        public async Task Handle(DeleteTaskAttributeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var attributes = await taskAttributeRepository.FindAllAsync(x => x.TaskID.Equals(request.TaskId), cancellationToken);
                foreach(var item in attributes)
                {
                    item.Status = false;
                    taskAttributeRepository.Update(item);
                }
            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
