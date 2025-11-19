using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Stage.CreateStage;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Enums;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.CreateMethod
{
    //create method command
    public class CreateMethodCommand : IRequest<string>, ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public List<CreateStageCommand> Stages { get; set; }
        public CreateMethodCommand(string name, string description,
            int type, List<CreateStageCommand> stages)
        {
            Name = name;
            Description = description;
            Type = type;
            Stages = stages;
        }
    }

    internal class CreateMethodCommandHandler(IMethodRepository methodRepository, IMediator sender) : IRequestHandler<CreateMethodCommand, string>
    {
        public async Task<string> Handle(CreateMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var methodTypeEnum = (MethodType)request.Type;
                var methodTypeString = methodTypeEnum.ToString();
                Methods obj = new()
                {
                    Name = request.Name,
                    Status = true,
                    Description = request.Description,
                    Type = methodTypeString,
                };
                methodRepository.Add(obj);

                foreach(var stageCommand in request.Stages)
                {
                    //Only inject the command to obey DI principles, the handler will be executed by the MediatR pipeline
                    stageCommand.MethodID = obj.ID;
                    await sender.Send(stageCommand, cancellationToken);
                }

                return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Tạo method thành công với tên : {obj.Name}" : "Tạo thất bại.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
