using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.StateChangeExperimentLog
{
    public class StateChangeExperimentLogCommand : IRequest<string>, ICommand
    {
        public string ELID { get; set; }
        public StateChangeExperimentLogCommand(string ELID)
        {
            this.ELID = ELID;
        }
    }

    internal class StateChangeExperimentLogCommandHandler(IStageRepository stageRepository,
        ILinkedRepository linkedRepository,
        IExperimentLogRepository experimentLogRepository,
        ISampleRepository sampleRepository,
        IReportRepository reportRepository,
        ITissueCultureBatchRepository tissueCultureBatchRepository
        ) : IRequestHandler<StateChangeExperimentLogCommand, string>
    {

        public async Task<string> Handle(StateChangeExperimentLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var eL = await experimentLogRepository.FindAsync(x => x.ID.Equals(request.ELID), cancellationToken);
                var stageStep = (await stageRepository.FindAsync(x => x.ID.Equals(eL.CurrentStageID), cancellationToken))?.Step;
                stageStep += 1;
                var nextStage = await stageRepository.FindAsync(x => x.MethodID.Equals(eL.MethodID) && x.Step == stageStep, cancellationToken);
                if (nextStage == null)
                {
                    eL!.Status = 1;
                    List<Domain.Entities.Linkeds> existingLinked = await linkedRepository.FindAllAsync(x => x.ExperimentLogID.Equals(eL.ID) && x.StageID.Equals(eL.CurrentStageID), cancellationToken);
                    foreach (var linked in existingLinked)
                    {
                        linked.ProcessStatus = 1;
                        linkedRepository.Update(linked);
                    }
                    var tissue = await tissueCultureBatchRepository.FindAsync(x => x.ID.Equals(eL.TissueCultureBatchID), cancellationToken);
                    tissue.Status = true;
                    tissueCultureBatchRepository.Update(tissue);
                }
                else
                {
                    List<Domain.Entities.Samples> sample = await sampleRepository.FindAllAsync(x => x.Linkeds.Any(x => x.ExperimentLogID.Equals(eL.ID)), cancellationToken);

                    var sampleIds = sample.Select(x => x.ID).ToList();

                    var existedReports = await reportRepository.FindAllAsync(
                        r => sampleIds.Contains(r.SampleID),
                        cancellationToken);

                    var sampleWithReport = existedReports.Select(r => r.SampleID).ToHashSet();

                    var sampleWithoutReport = sampleIds
                        .Where(id => !sampleWithReport.Contains(id))
                        .ToList();

                    if (sampleWithoutReport.Count > 0)
                        throw new ArgumentException("Một vài sample chưa có report");

                    List<Domain.Entities.Linkeds> existingLinkeds = await linkedRepository.FindAllAsync(x => x.ExperimentLogID.Equals(eL.ID) && x.StageID.Equals(eL.CurrentStageID), cancellationToken);
                    List<Domain.Entities.Linkeds> newLinkeds = [];
                    existingLinkeds.ForEach(x => x.ProcessStatus = 1);
                    sample.ForEach(sample =>
                    {
                        newLinkeds.Add(new Domain.Entities.Linkeds
                        {
                            ExperimentLogID = eL.ID,
                            SampleID = sample.ID,
                            StageID = nextStage.ID,
                            ProcessStatus = 0,
                        });
                    });
                    linkedRepository.UpdateRange(existingLinkeds);
                    linkedRepository.AddRange(newLinkeds);
                    eL!.CurrentStageID = nextStage.ID;
                }
                experimentLogRepository.Update(eL);

                return await experimentLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Successed" : "Failed";
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
