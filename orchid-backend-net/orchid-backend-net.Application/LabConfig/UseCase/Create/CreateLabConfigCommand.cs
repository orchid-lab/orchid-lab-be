using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabConfig.UseCase.Create
{
    public record CreateLabConfigCommand(string ConfigName, string Key, decimal Value) : IRequest<string>;
    internal class CreateLabConfigCommandHandler(IConfigRepository configRepository) : IRequestHandler<CreateLabConfigCommand, string>
    {
        public async Task<string> Handle(CreateLabConfigCommand request, CancellationToken cancellationToken)
        {
            var config = await configRepository.FindAsync(c => 
            c.ConfigName.Equals(request.ConfigName) 
            && c.Key.Equals(request.Key), cancellationToken);
            if(config is not null)
            {
                throw new DuplicateException("Config này đã tồn tại.");
            }
            var newConfig = new Domain.Entities.Config
            {
                ConfigName = request.ConfigName,
                Key = request.Key,
                Value = request.Value
            };
            configRepository.Add(newConfig);
            return await configRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? newConfig.ID.ToString() 
                : "Thất bại";
        }
    }
}
