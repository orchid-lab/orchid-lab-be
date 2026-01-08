using MediatR;
using orchid_backend_net.Application.Method.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.DeleteSampleRequirementFromMethodStage
{
    public record DeleteSampleRequirementFromMethodStageCommand(int MethodId, int MethodStageId, string SampleRequirementId) : IRequest<string>;
    internal class DeleteSampleRequirementFromMethodStageCommandHandler(
        IMethodRepository methodRepository,
        ISampleRequirementDefinitionRepository sampleRequirementDefinitionRepository)
        : IRequestHandler<DeleteSampleRequirementFromMethodStageCommand, string>
    {
        public async Task<string> Handle(DeleteSampleRequirementFromMethodStageCommand request, CancellationToken cancellationToken)
        {
            await MethodPolicy.EnsureStageExistsAsync(methodRepository, request.MethodId, request.MethodStageId, cancellationToken);

            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            await MethodPolicy.EnsureSampleRequirementExistsAsync(
                sampleRequirementDefinitionRepository, 
                request.SampleRequirementId, 
                cancellationToken);

            method.RemoveSampleRequirementFromStage(request.MethodStageId, request.SampleRequirementId);
            methodRepository.Update(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Xóa sample requirement thành công." :
                "Xóa sample requirement thất bại";
        }
    }
}
