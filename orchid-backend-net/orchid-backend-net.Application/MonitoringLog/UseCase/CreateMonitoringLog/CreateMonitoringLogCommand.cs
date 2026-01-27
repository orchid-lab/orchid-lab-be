using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.MonitoringLog.UseCase.CreateMonitoringLog
{
    public record CreateMonitoringLogCommand(string Name, string SampleStageId, string? AnalyticResultId, string? DiseaseId) : IRequest<string>;
    internal class CreateMonitoringLogCommandHandler(
        IMonitoringLogRepository monitoringLogRepository,
        ISampleRepository sampleRepository,
        ICurrentUserService currentUserService,
        IDiseaseRepository diseaseRepository,
        IAnalyticResultRepository analyticResultRepository
        ) : IRequestHandler<CreateMonitoringLogCommand, string>
    {
        public async Task<string> Handle(CreateMonitoringLogCommand request, CancellationToken cancellationToken)
        {
            var sample = await sampleRepository
                .FindAsync(s => 
                s.SampleStages.Any(ss => ss.ID == request.SampleStageId),
                cancellationToken);

            var sampleStage = sample.SampleStages.FirstOrDefault(s => s.ID == request.SampleStageId);

            var sampleStageDefinitionRequirement = sampleStage.SampleStageDefinition.StageRequirementDefinitions;
        }
    }
}
