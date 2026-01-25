using MediatR;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.ChangeSampleStage
{
    public record ChangeSampleStageCommand(string SampleId) : IRequest<string>;
    internal class ChangeSampleStageCommandHandler(
        ISampleRepository sampleRepository,
        ISampleStageDefinitionRepository sampleStageDefinitionRepository)
        : IRequestHandler<ChangeSampleStageCommand, string>
    {
        public async Task<string> Handle(ChangeSampleStageCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository.FindAsync(s => s.ID.Equals(request.SampleId), cancellationToken)
                ?? throw new NotFoundException("Sample không tồn tại");

            var stageDefinition = await sampleStageDefinitionRepository.GetOrderDefinitionIdsAsync(cancellationToken);

            sample.CompleteCurrentStage(stageDefinition);

            sampleRepository.Update(sample);
            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? sample.ID.ToString()
                : "Chuyển giai đoạn sample thất bại";
        }
    }
}
