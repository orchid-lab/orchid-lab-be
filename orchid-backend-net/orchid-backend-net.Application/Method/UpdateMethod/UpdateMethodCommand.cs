using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Stage.UpdateStage;
using orchid_backend_net.Domain.Enums;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UpdateMethod
{
    public class UpdateMethodCommand(string id, string name, string description, int type) : IRequest<string>, ICommand
    {
        public string ID { get; set; } = id;
        public string? Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public int? Type { get; set; } = type;
        public List<UpdateStageCommand> Stages { get; set; }

    }

    internal class UpdateMethodCommandHandler(IMethodRepository methodRepository, ISender sender) : IRequestHandler<UpdateMethodCommand, string>
    {
        public async Task<string> Handle(UpdateMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {


                var method = await methodRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status, cancellationToken);

                string methodTypeString = "";
                if (request.Type == 0 || request.Type == null)
                    methodTypeString = method.Type;
                else
                {
                    var methodTypeEnum = (MethodType)request.Type;
                    methodTypeString = methodTypeEnum.ToString();
                }

                method.Name = request.Name ?? method.Name;
                method.Description = request.Description ?? method.Description;
                method.Type = methodTypeString;
                methodRepository.Update(method);

                //Update Method means update the whole method and stages, element in stage, referents in the stage
                if (request.Stages.Count > 0)
                {
                    foreach (var stageCommand in request.Stages)
                    {
                        //Only inject the command to obey DI principles, the handler will be executed by the MediatR pipeline
                        stageCommand.MethodID = method.ID;
                        await sender.Send(stageCommand, cancellationToken);
                    }
                }

                return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                    ? $"Updated Method with ID :{request.ID}" : $"Failed to update Method with ID :{request.ID}";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

    }
}
