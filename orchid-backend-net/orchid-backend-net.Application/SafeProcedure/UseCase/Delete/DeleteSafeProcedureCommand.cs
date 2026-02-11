using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.SafeProcedure.UseCase.Delete
{
    public record DeleteSafeProcedureCommand(string Id) : IRequest<string>;
    internal class DeleteSafeProcedureCommandHandler
        (ISafeProcedureRepository safeProcedureRepository,
        ICurrentUserService currentUserService)
        : IRequestHandler<DeleteSafeProcedureCommand, string>
    {
        public async Task<string> Handle(DeleteSafeProcedureCommand request, CancellationToken cancellationToken)
        {
            var safeProcedure = await safeProcedureRepository.FindAsync(
                c => c.ID.Equals(request.Id), cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy cơ chế an toàn với ID đã cho.");
            safeProcedure.DeletedBy = currentUserService.UserId;
            safeProcedure.DeletedDate = DateTime.UtcNow;
            safeProcedureRepository.Update(safeProcedure);
            return await safeProcedureRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? safeProcedure.ID.ToString()
                : "Xoá thất bại";
        }
    }
}
