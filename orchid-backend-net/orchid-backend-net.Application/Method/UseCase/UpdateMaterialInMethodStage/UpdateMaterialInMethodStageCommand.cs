using MediatR;
using orchid_backend_net.Application.Method.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.UpdateMaterialInMethodStage
{
    public record UpdateMaterialInMethodStageCommand(
        int MethodId,
        int MethodStageId, 
        string StageMaterialId, 
        int? MaterialId) : IRequest<string>;
    internal class UpdateMaterialInMethodStageCommandHandler(
        IMethodRepository methodRepository,
        IMaterialRepository materialRepository) : IRequestHandler<UpdateMaterialInMethodStageCommand, string>
    {
        public async Task<string> Handle(UpdateMaterialInMethodStageCommand request, CancellationToken cancellationToken)
        {
            await MethodPolicy.EnsureStageExistsAsync(methodRepository, request.MethodId, request.MethodStageId, cancellationToken);

            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            if (request.MaterialId.HasValue)
                await MethodPolicy.EnsureMaterialExistsAsync(materialRepository, request.MaterialId.Value, cancellationToken);

            method.UpdateMaterialInStage(request.MethodStageId, request.StageMaterialId, request.MaterialId);
            methodRepository.Update(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Cập nhật thành công." :
                "Cập nhật thất bại";
        }
    }
}
