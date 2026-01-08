using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.UpdateMethod
{
    public record UpdateMethodInformationCommand(int MethodId, string? MethodName, string? MethodDescription) : IRequest<string>;
    internal class UpdateMethodInformationCommandHandler(IMethodRepository methodRepository) : IRequestHandler<UpdateMethodInformationCommand, string>
    {
        public async Task<string> Handle(UpdateMethodInformationCommand request, CancellationToken cancellationToken)
        {
            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
               ?? throw new NotFoundException("Không tìm thấy method này");

            method.Name = request.MethodName ?? method.Name;
            method.Description = request.MethodDescription ?? method.Description;
            methodRepository.Update(method);

            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? "Cập nhật thông tin thành công."
                : "Cập nhật thông tin thất bại";
        }
    }
}
