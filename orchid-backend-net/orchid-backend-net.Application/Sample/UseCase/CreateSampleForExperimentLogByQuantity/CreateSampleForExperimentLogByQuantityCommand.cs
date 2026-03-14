using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Sample.Helper;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Sample.UseCase.CreateSampleByQuantity
{
    public record CreateSampleForExperimentLogByQuantityCommand(string ExperimentLogId, int Quantity, string? InitialCondition) : IRequest<string>;

    internal class CreateSampleForExperimentLogByQuantityCommandHandler(
        ISampleRepository sampleRepository,
        IExperimentLogRepository experimentLogRepository,
        ISampleStageDefinitionRepository sampleStageDefinitionRepository,
        ICurrentUserService currentUserService) : IRequestHandler<CreateSampleForExperimentLogByQuantityCommand, string>
    {
        public async Task<string> Handle(CreateSampleForExperimentLogByQuantityCommand request, CancellationToken cancellationToken)
        {
            var experiment = 
                await experimentLogRepository.GetExperimentLogByIdAsync(request.ExperimentLogId, cancellationToken);
            var firstStageDefinition = 
                await sampleStageDefinitionRepository.GetFirstStageDefinitionIdAsync(cancellationToken);
            var sampleList = CreateSampleByQuantityHelper.CreateMultipleSample(
                experiment.Name,
                experiment.ID,
                firstStageDefinition,
                request.Quantity,
                currentUserService.UserId,
                request.InitialCondition);
            experiment.UpdatedDate = DateTime.UtcNow;
            experiment.UpdatedBy = currentUserService.UserId;
            sampleRepository.AddRange(sampleList);
            return await sampleRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 
                ? "Tạo sample thành công"
                : "Tạo thất bại";
        }
    }
}
