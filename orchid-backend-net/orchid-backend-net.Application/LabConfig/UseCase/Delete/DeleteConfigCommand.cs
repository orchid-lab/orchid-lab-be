using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.LabConfig.UseCase.Delete
{
    public record DeleteConfigCommand(string Id) : IRequest<string>;
    internal class DeleteConfigCommandHandler(IConfigRepository configRepository) : IRequestHandler<DeleteConfigCommand, string>
    {
        public async Task<string> Handle(DeleteConfigCommand request, CancellationToken cancellationToken)
        {
            var config = await configRepository.FindAsync(c => c.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy config này");
            configRepository.Remove(config);
            return await configRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? config.ID
                : "Xoá config thất bại";
        }
    }
}