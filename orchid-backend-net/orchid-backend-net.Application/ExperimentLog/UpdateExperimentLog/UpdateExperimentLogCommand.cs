using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.UpdateExperimentLog
{
    public class UpdateExperimentLogCommand : IRequest<string>, ICommand
    {
        public string ID { get; set; }
        public string? MethodID { get; set; }
        public string Description { get; set; }
        public string? TissueCultureBatchID { get; set; }
        public int Status { get; set; }
        public List<string>? Hybridization { get; set; }
        public UpdateExperimentLogCommand(string id, string? methodID, string description, string? tissueCultureBatchID, int status, List<string>? hybridization)
        {
            ID = id;
            MethodID = methodID;
            Description = description;
            TissueCultureBatchID = tissueCultureBatchID;
            Status = status;
            Hybridization = hybridization;
        }
    }

    internal class UpdateExperimentLogCommandHandler(IExperimentLogRepository experimentLogRepository, IHybridizationRepository hybridizationRepository, ISeedlingRepository seedlingRepository) : IRequestHandler<UpdateExperimentLogCommand, string>
    {

        public async Task<string> Handle(UpdateExperimentLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var experimentLog = await experimentLogRepository.FindAsync(x => x.ID.Equals(request.ID) && x.Status != 4, cancellationToken);
                if (request.MethodID != null)
                {
                    experimentLog.MethodID = request.MethodID;
                }
                if (request.TissueCultureBatchID != null)
                {
                    experimentLog.TissueCultureBatchID = request.TissueCultureBatchID;
                }
                experimentLog.Description = request.Description;
                experimentLog.Status = request.Status;
                experimentLogRepository.Update(experimentLog);
                var hybridization = await hybridizationRepository.FindAllAsync(x => x.ExperimentLogID.Equals(request.ID) && x.Status, cancellationToken);
                foreach (var hybridizationItem in hybridization)
                {
                    foreach (var parent in request.Hybridization)
                    {
                        hybridizationItem.ParentID = parent;
                        hybridizationRepository.Update(hybridizationItem);
                    }
                }
                await hybridizationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Updated ExperimentLog with ID :{request.ID}" : $"Failed to update ExperimentLog with ID :{request.ID}";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
