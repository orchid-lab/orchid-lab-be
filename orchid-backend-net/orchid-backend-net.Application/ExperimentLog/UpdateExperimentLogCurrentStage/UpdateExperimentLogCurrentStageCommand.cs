using MediatR;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UpdateExperimentLogCurrentStage
{
    public class UpdateExperimentLogCurrentStageCommand(string experimentLogId, string currentStage) : IRequest<string>
    {
        public string ExperimentLogId { get; set; } = experimentLogId;
        public string CurrentStage { get; set; } = currentStage;
    }

    internal class UpdateExperimentLogCurrentStageCommandHandler(IExperimentLogRepository experimentLogRepository) : IRequestHandler<UpdateExperimentLogCurrentStageCommand, string>
    {
        public async Task<string> Handle(UpdateExperimentLogCurrentStageCommand request, CancellationToken cancellationToken)
        {
            var experimentLog = await experimentLogRepository.FindAsync(x => x.ID.Equals(request.ExperimentLogId), cancellationToken);
            experimentLog.CurrentStageID = request.CurrentStage;
            experimentLogRepository.Update(experimentLog);
            return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? experimentLog.ID : string.Empty;
        }
    }
}
