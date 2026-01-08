using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.DeleteMethod
{
    public record DeleteMethodCommand(int Id) : IRequest<string>;

    internal class DeleteMethodCommandHandler(IMethodRepository methodRepository) : IRequestHandler<DeleteMethodCommand, string>
    {
        public async Task<string> Handle(DeleteMethodCommand request, CancellationToken cancellationToken)
        {
            var method = await methodRepository.FindAsync(m => m.ID == request.Id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            methodRepository.Remove(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 
                ? "Xóa thành công" :
                "Xóa thất bại";
        }
    }
}
