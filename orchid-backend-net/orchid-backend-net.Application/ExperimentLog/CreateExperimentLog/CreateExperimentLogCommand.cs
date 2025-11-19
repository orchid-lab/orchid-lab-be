using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Hybridzations;
using orchid_backend_net.Application.Sample.GenerateSample;
using orchid_backend_net.Application.Tasks.GenerateTask;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.CreateExperimentLog
{
    public class CreateExperimentLogCommand : IRequest<string>, ICommand
    {
        public string Name { get; set; }
        public string MethodID { get; set; }
        public string Description { get; set; }
        public string TissueCultureBatchID { get; set; }
        public List<string> Hybridization { get; set; }
        public int NumberOfSample { get; set; }
        public List<string> TechnicianID { get; set; }
    }

    internal class CreateExperimentLogCommandHandler(IExperimentLogRepository experimentLogRepository, IHybridizationRepository hybridizationRepository,
        IStageRepository stageRepository, ICurrentUserService currentUserService, ISender sender) : IRequestHandler<CreateExperimentLogCommand, string>
    {
        public async Task<string> Handle(CreateExperimentLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //get stage
                var stage = await stageRepository.FindAsync(x => x.MethodID.Equals(request.MethodID) && x.Status && x.Step == 1, cancellationToken);

                ExperimentLogs obj = new()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    CurrentStageID = stage.ID, // Assuming the first stage is the current stage
                    MethodID = request.MethodID,
                    Description = request.Description,
                    TissueCultureBatchID = request.TissueCultureBatchID,
                    Status = 0,
                    Create_date = DateTime.UtcNow,
                    Create_by = currentUserService.UserName,
                };

                await Task.WhenAll(
                    sender.Send(new CreateHybridzationCommand(request.Hybridization, obj.ID), cancellationToken),
                    sender.Send(new GenerateSampleCommand(obj.Name, obj.ID, stage.ID, request.NumberOfSample), cancellationToken),
                    sender.Send(new GenerateTaskCommand()
                    {
                        ExperimentLogId = obj.ID,
                        StageId = stage.ID,
                        TechnicianID = request.TechnicianID
                    }, cancellationToken)
                );

                experimentLogRepository.Add(obj);

                return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? $"Created ExperimentLog with ID: {obj.ID}" : "Failed to create ExperimentLog.";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
