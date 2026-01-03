using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Seedling.RemoveSeedlings
{
    public class RemoveSeedlingsCommand(string id) : IRequest<string>
    {
        public required string Id { get; set; } = id;
    }

    internal class RemoveSeedlingsCommandHandler(ISeedlingRepository seedlingRepository, ICurrentUserService currentUserService) : IRequestHandler<RemoveSeedlingsCommand, string>
    {
        public async Task<string> Handle(RemoveSeedlingsCommand request, CancellationToken cancellationToken)
        {
            var seedling = await seedlingRepository.FindAsync(x => x.ID.Equals(request.Id), cancellationToken);
            if (seedling == null)
            {
                throw new NotFoundException("Cây giống không tồn tại.");
            }
            seedling.DeletedDate = DateTime.UtcNow;
            seedling.DeletedBy = currentUserService.UserId;
            seedlingRepository.Update(seedling);
            return await seedlingRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Xóa cây giống thành công." : "Xóa cây giống thất bại.";
        }
    }
}
