using MediatR;
using orchid_backend_net.Application.Method.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.DeleteChemicalFromMethodStage
{
    public record DeleteChemicalFromMethodStageCommand(int MethodId, int MethodStageId, int ChemicalId) : IRequest<string>;
    internal class RemoveChemicalFromMethodStageCommandHandler(
        IMethodRepository methodRepository,
        IChemicalsRepository chemicalsRepository) : IRequestHandler<DeleteChemicalFromMethodStageCommand, string>
    {
        public async Task<string> Handle(DeleteChemicalFromMethodStageCommand request, CancellationToken cancellationToken)
        {
            await MethodPolicy.EnsureStageExistsAsync(methodRepository, request.MethodId, request.MethodStageId, cancellationToken);

            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            await MethodPolicy.EnsureChemicalExistsAsync(chemicalsRepository, request.ChemicalId, cancellationToken);
            
            method.RemoveChemicalFromStage(request.MethodStageId, request.ChemicalId);
            
            methodRepository.Update(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Xóa chemical thành công." :
                "Xóa chemical thất bại";
        }
    }
}
