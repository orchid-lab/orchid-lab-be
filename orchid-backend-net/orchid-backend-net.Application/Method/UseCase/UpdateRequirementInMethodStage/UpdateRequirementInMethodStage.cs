using MediatR;
using orchid_backend_net.Application.Method.Policy;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Method.UseCase.UpdateRequirementInMethodStage
{
    public record UpdateRequirementInMethodStageCommand(
        int MethodId,
        int MethodStageId,
        string SampleRequirementId,
        decimal? Minvalue,
        decimal? MaxValue,
        decimal? ExpectedValue) : IRequest<string>;
    internal class UpdateMethodRequirementInStageCommandHandler(
        IMethodRepository methodRepository,
        ISampleRequirementDefinitionRepository sampleRequirementDefinitionRepository) : IRequestHandler<UpdateRequirementInMethodStageCommand, string>
    {
        public async Task<string> Handle(UpdateRequirementInMethodStageCommand request, CancellationToken cancellationToken)
        {
            await MethodPolicy.EnsureStageExistsAsync(methodRepository, request.MethodId, request.MethodStageId, cancellationToken);

            var method = await methodRepository.FindAsync(m => m.ID == request.MethodId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy method này");

            await MethodPolicy.EnsureSampleRequirementExistsAsync(sampleRequirementDefinitionRepository, request.SampleRequirementId, cancellationToken);

            method.UpdateSampleRequirementInStage(request.MethodStageId, request.SampleRequirementId, new Domain.Entities.UpdateSampleRequirementSpec()
            {
                MinValue = request.Minvalue,
                MaxValue = request.MaxValue,
                ExpectedValue = request.ExpectedValue
            });
            methodRepository.Update(method);
            return await methodRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ?
                "Cập nhật thành công." :
                "Cập nhật thất bại";
        }
    }
}
