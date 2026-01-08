using MediatR;
using orchid_backend_net.Application.Method.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.DeleteMaterialFromMethodStage
{
    public record DeleteMaterialFromMethodStageCommand(int MethodId, int MethodStageId, int MaterialId) : IRequest<string>;
    internal class RemoveMaterialFromMethodStageCommandHandler(
        IMethodRepository methodRepository,
        IMaterialRepository materialRepository) : IRequestHandler<DeleteMaterialFromMethodStageCommand, string>
    {
        public async Task<string> Handle(DeleteMaterialFromMethodStageCommand request, CancellationToken cancellationToken)
        {
            await MethodPolicy.EnsureStageExistsAsync(methodRepository, request.MethodId, request.MethodStageId, cancellationToken);

            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            await MethodPolicy.EnsureMaterialExistsAsync(materialRepository, request.MaterialId, cancellationToken);
            
            method.RemoveMaterialFromStage(request.MethodStageId, request.MaterialId);
            
            methodRepository.Update(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Xóa material thành công." :
                "Xóa material thất bại";
        }
    }
}
