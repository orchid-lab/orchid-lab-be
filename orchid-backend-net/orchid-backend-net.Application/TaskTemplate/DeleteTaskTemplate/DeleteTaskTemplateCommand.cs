using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.TaskTemplate.DeleteTaskTemplate
{
    public class DeleteTaskTemplateCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public DeleteTaskTemplateCommand(string ID)
        {
            this.ID = ID;
        }
    }
    internal class DeleteTaskTemplateCommandHandler(ITaskTemplatesRepository taskTemplateRepository) : IRequestHandler<DeleteTaskTemplateCommand, string>
    {
        public async Task<string> Handle(DeleteTaskTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskTemplate = await taskTemplateRepository.FindAsync(x => x.ID.Equals(request.ID), cancellationToken);
                if (taskTemplate != null)
                    throw new NotFoundException($"Not found ask template with ID {request.ID}");
                taskTemplate.Status = false;
                taskTemplateRepository.Update(taskTemplate);
                return (await taskTemplateRepository.UnitOfWork.SaveChangesAsync()) > 0 ? $"Deleted task template ID : {request.ID} /n Name: {taskTemplate.Name}" : "Failed to detele task template.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
