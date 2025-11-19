using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Infrastructure.Service.HostedService
{
    public class ExperimentLogHostedService(IServiceProvider serviceProvider, ILogger<ExperimentLogHostedService> logger) : IHostedService, IDisposable
    {
        private Timer timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // run at 2AM
            var now = DateTime.Now;
            var nextRun = now.Date.AddHours(2); // 2:00 AM
            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            var delay = nextRun - now;

            logger.LogInformation("StageTransitionService will run first time at: {NextRun}", nextRun);

            timer = new Timer(DoWork, null, delay, TimeSpan.FromDays(1)); // Once a day
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            logger.LogInformation("StageTransitionService is checking stage transition for each experiment log");

            using var scope = serviceProvider.CreateScope();
            var experimentLogsRepo = scope.ServiceProvider.GetRequiredService<IExperimentLogRepository>();
            var methodRepo = scope.ServiceProvider.GetRequiredService<IMethodRepository>();
            var stageRepo = scope.ServiceProvider.GetRequiredService<IStageRepository>();
            var linkedRepo = scope.ServiceProvider.GetRequiredService<ILinkedRepository>();
            var reportsRepo = scope.ServiceProvider.GetRequiredService<IReportRepository>();
            var cancellationToken = new CancellationToken();

            // Call the method to process stage transitions
            try
            {
                var allLogs = await experimentLogsRepo.FindAllAsync(cancellationToken);
                foreach (var log in allLogs)
                {
                    await ProcessExperimentLog(log, experimentLogsRepo, methodRepo, stageRepo, linkedRepo, reportsRepo, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing stage transitions.");
            }
        }

        private async Task ProcessExperimentLog(ExperimentLogs log, IExperimentLogRepository experimentLogRepo, IMethodRepository methodRepo, IStageRepository stageRepo, ILinkedRepository linkedRepo, IReportRepository reportRepo, CancellationToken cancellationToken)
        {

            //declare start date and target date of stage
            DateOnly experimentLogStartDays = DateOnly.FromDateTime((DateTime)log.Create_date!);
            DateOnly targetDate = new();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var totalProcessingDays = 0;

            //get the method and all stage of that method
            var method = await methodRepo.FindAsync(x => x.ID.Equals(log.MethodID) && x.Status == true, cancellationToken);
            if (method == null)
                return;

            var stages = await stageRepo.FindAllAsync(x => x.MethodID.Equals(method!.ID) && x.Status == true, cancellationToken);
            if (stages == null || stages.Count == 0)
                return; //skip if stages is null or empty

            //get the current stage of experiment log
            var currentStep = 0;
            var currentStage = await stageRepo.FindAsync(x => x.ID.Equals(log.CurrentStageID) && x.Status == true, cancellationToken);
            if (currentStage == null)
                return; //skip if current stage is null

            //else set the step in to current stage step
            currentStep = currentStage.Step;

            //check current stage is the step one or else
            if (currentStep == 1)
                targetDate = experimentLogStartDays.AddDays(currentStage.DateOfProcessing);
            else
            {
                totalProcessingDays = stages.Where(x => x.Step <= currentStep).Sum(x => x.DateOfProcessing);
                targetDate = experimentLogStartDays.AddDays(totalProcessingDays);
            }

            if (currentDate < targetDate)
                return;
            else
            {
                //get the unique linkeds to get sample
                var duplicatedFollowExperimentLogStageLinkeds = await linkedRepo.FindAllAsync(x => x.ExperimentLogID!.Equals(log.ID) && x.ProcessStatus == 0 && x.StageID!.Equals(currentStage.ID), cancellationToken);

                //get unique linkeds by sample id
                var uniqueFollowExperimentLogStageLinkeds = duplicatedFollowExperimentLogStageLinkeds.GroupBy(x => x.SampleID).Select(g => g.First()).ToList();
                var allSamplesCount = uniqueFollowExperimentLogStageLinkeds.Count;

                var sampleIds = uniqueFollowExperimentLogStageLinkeds
                    .Where(l => l.SampleID != null)
                    .Select(l => l.SampleID!)
                    .ToList();

                var existingReports = await reportRepo.FindAllAsync(
                    r => sampleIds.Contains(r.SampleID),
                    cancellationToken);

                var samplesWithReport = existingReports.Select(r => r.SampleID).ToHashSet();

                var samplesWithoutReport = sampleIds
                    .Where(id => !samplesWithReport.Contains(id))
                    .ToList();

                if (samplesWithoutReport.Count > 0)
                {
                    logger.LogWarning("Some samples do not have reports in ExperimentLog ID: {ExperimentLogID}. Sample IDs: {SampleIDs}", log.ID, string.Join(", ", samplesWithoutReport));
                    logger.LogWarning("Cannot proceed with stage transition for these samples.");
                    return; //skip the process if some samples do not have reports
                }

                //get next stage by step
                var nextStage = stages.FirstOrDefault(x => x.Step == currentStep + 1);
                if (nextStage == null)
                {
                    log.Update_date = DateTime.UtcNow;
                    log.Update_by = "System";
                    log.Status = 1; //mark as done
                    experimentLogRepo.Update(log);
                    return;
                }

                //update the experiment log to next stage
                log.CurrentStageID = nextStage.ID;
                log.Update_date = DateTime.UtcNow;
                log.Update_by = "System";
                experimentLogRepo.Update(log);


                //update the linkeds into done
                duplicatedFollowExperimentLogStageLinkeds.ForEach(x => x.ProcessStatus = 1);
                linkedRepo.UpdateRange(duplicatedFollowExperimentLogStageLinkeds);

                //create new linkeds due to next stage
                List<Linkeds> newLinkedsSampleForExperimentLogStage = [];
                uniqueFollowExperimentLogStageLinkeds.ForEach(unique =>
                {
                    newLinkedsSampleForExperimentLogStage.Add(new Linkeds
                    {
                        ExperimentLogID = log.ID,
                        SampleID = unique.SampleID,
                        StageID = nextStage.ID,
                        ProcessStatus = 0,
                    });
                });

                linkedRepo.AddRange(newLinkedsSampleForExperimentLogStage);
            }
            await experimentLogRepo.UnitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StageTransitionService is stopping");
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
