using MediatR;
using orchid_backend_net.Application.Method.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.UpdateChemicalInMethodStage
{
    public record UpdateChemicalInMethodStage(
        int MethodId,
        int MethodStageId,
        string StageMaterialId,
        int? ChemicalId) : IRequest<string>;

    internal class UpdateChemicalInMethodStageCommandHandler(
        IMethodRepository methodRepository,
        IChemicalsRepository chemicalsRepository) : IRequestHandler<UpdateChemicalInMethodStage, string>
    {
        public async Task<string> Handle(UpdateChemicalInMethodStage request, CancellationToken cancellationToken)
        {
            await MethodPolicy.EnsureStageExistsAsync(methodRepository, request.MethodId, request.MethodStageId, cancellationToken);

            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            if(request.ChemicalId.HasValue) 
                await MethodPolicy.EnsureChemicalExistsAsync(chemicalsRepository, request.ChemicalId.Value, cancellationToken);

            method.UpdateChemicalInStage(request.MethodStageId, request.StageMaterialId, request.ChemicalId);
            methodRepository.Update(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Cập nhật thành công." :
                "Cập nhật thất bại";
        }
    }
}
