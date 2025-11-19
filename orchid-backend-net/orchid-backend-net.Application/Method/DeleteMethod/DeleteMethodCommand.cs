using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Stage.DeleteStage;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.DeleteMethod
{
    public class DeleteMethodCommand(string ID) : IRequest<string>, ICommand
    {
        public string ID { get; set; } = ID;
    }

    internal class DeleteMethodCommandHandler(IMethodRepository methodRepository, ISender sender) : IRequestHandler<DeleteMethodCommand, string>
    {
        public async Task<string> Handle(DeleteMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var method = await methodRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status == true, cancellationToken);
                method.Status = false;
                methodRepository.Update(method);

                //Send to stage to deactivate all stages of this method
                await sender.Send(new DeleteStageCommand(method.ID), cancellationToken);
                return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Deleted method name :{method.Name}" : "Failed to delete method.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
