using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabConfig.UseCase.Update
{
    public record UpdateConfigCommand(string Id, string? ConfigName, string? Key, decimal? Value) : IRequest<string>;
    internal class UpdateConfigCommandHandler(IConfigRepository configRepository) : IRequestHandler<UpdateConfigCommand, string>
    {
        public async Task<string> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
        {
            var config = await configRepository.FindAsync(c => c.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy");
            config.ConfigName = request.ConfigName ?? config.ConfigName;
            config.Key = request.Key ?? config.Key;
            config.Value = request.Value ?? config.Value;
            configRepository.Update(config);
            return await configRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? config.ID
                : "Thất bại";
        }
    }
}
