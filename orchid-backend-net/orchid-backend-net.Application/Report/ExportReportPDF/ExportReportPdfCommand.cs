using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Report.ExportReportPDF
{
    public class ExportReportPdfCommand(string experimentLogId) : IRequest<byte[]>, ICommand
    {
        public string ExperimentLogId { get; set; } = experimentLogId;
    }

    internal class ExportReportPdfCommandHandler(
        IPdfReportGenerator pdfReportGenerator, 
        IExperimentLogRepository experimentLogRepository,
        IMethodRepository methodRepository, 
        IHybridizationRepository hybridizationRepository, 
        ISeedlingRepository seedlingRepository,
        ISampleRepository sampleRepository,
        IInfectedSampleRepository infectedSampleRepository,
        ILinkedRepository linkedRepository,
        ITaskRepository taskRepository,
        ITaskAssignRepository taskAssignRepository,
        ITaskAttributeRepository taskAttributeRepository,
        IReportRepository reportRepository,
        IReportAttributeRepository reportAttributeRepository,
        IUserRepository userRepository) : IRequestHandler<ExportReportPdfCommand, byte[]>
    {
        public async Task<byte[]> Handle(ExportReportPdfCommand request, CancellationToken cancellationToken)
        {
            //get experiment log data when experiment log is done
            var experimentLogData = await experimentLogRepository.FindAsync(eL => eL.ID.Equals(request.ExperimentLogId) && eL.Status == 1, cancellationToken);

            //get method based on experiment log
            var method = await methodRepository.FindAsync(m => m.ID.Equals(experimentLogData!.MethodID), cancellationToken);

            //calculate total days of experiment log based on method stages
            var totalDays = method!.Stages.Sum(stage => stage.DateOfProcessing);

            //get all linkeds and sort theo stage
            var allLinkeds = await linkedRepository.FindAllAsync(l => l.ExperimentLogID!.Equals(request.ExperimentLogId), cancellationToken);

            var linkedsByStage = allLinkeds
                .GroupBy(linkeds => linkeds.StageID)
                .ToDictionary(group => group.Key!, group => group.ToList());

            //build object with all details for each stage
            var stagesWithDetails = new List<object>();
            var allStageTasks = new List<Domain.Entities.Tasks>();
            var allStageSamples = new List<Samples>();
            foreach (var stage in method.Stages.OrderBy(stages => stages.Step))
            {
                var stagesLinkeds = linkedsByStage.GetValueOrDefault(stage.ID, []);

                //get task for current stage
                var stageTaskIds = stagesLinkeds
                    .Where(linkeds => linkeds.TaskID != null)
                    .Select(linkeds => linkeds.TaskID)
                    .ToList();

                var stageTasks = stageTaskIds.Any()
                    ? await taskRepository.FindAllAsync(t => stageTaskIds.Contains(t.ID), cancellationToken)
                    : [];

                //get task assign and task attribute for task 
                var taskAttribute = stageTaskIds.Any()
                    ? await taskAttributeRepository.FindAllAsync(ta => stageTaskIds.Contains(ta.TaskID), cancellationToken)
                    : [];

                var taskAssigns = stageTaskIds.Any()
                    ? await taskAssignRepository.FindAllAsync(ta => stageTaskIds.Contains(ta.TaskID), cancellationToken)
                    : [];

                var attributesByTask = taskAttribute
                    .GroupBy(ta => ta.TaskID)
                    .ToDictionary(group => group.Key!, group => group.ToList());

                var assignsByTask = taskAssigns
                    .GroupBy(ta => ta.TaskID)
                    .ToDictionary(group => group.Key!, group => group.ToList());

                //get task assign user
                var technicianIds = taskAssigns.Select(a => a.TechnicianID).Distinct().ToList();
                var technicians = technicianIds.Any()
                    ? (await userRepository.FindAllAsync(u => technicianIds.Contains(u.ID), cancellationToken))
                        .ToDictionary(u => u.ID, u => u.Name)
                    : [];

                //build task object with full attribute and assign
                var detailedTasks = stageTasks.Select(t => new
                {
                    TaskName = t.Name,
                    AssignedTo = assignsByTask.GetValueOrDefault(t.ID, new List<Domain.Entities.TasksAssign>())
                    .Select(a => technicians.GetValueOrDefault(a.TechnicianID, "N/A"))
                    .FirstOrDefault() ?? "N/A",
                    AssignedDate = t.Start_date,
                    CompletedDate = t.End_date,
                    Attributes = attributesByTask.GetValueOrDefault(t.ID, new List<TaskAttributes>())
                    .Select(a => new
                    {
                        Name = a.Name,
                        Value = a.Value,
                        Unit = a.MeasurementUnit
                    })
                    .ToList()
                }).ToList();

                //get sample for current stage
                var stageSampleIds = stagesLinkeds
                    .Where(l => l.SampleID != null)
                    .Select(l => l.SampleID!)
                    .ToList();

                var stageSamples = stageSampleIds.Any()
                    ? await sampleRepository.FindAllAsync(s => stageSampleIds.Contains(s.ID), cancellationToken)
                    : [];

                //get report for samples
                var sampleReports = stageSampleIds.Any()
                    ? await reportRepository.FindAllAsync(r => stageSampleIds.Contains(r.SampleID), cancellationToken)
                    : [];

                //get report attribute for reports
                var reportIds = sampleReports.Select(r => r.ID).ToList();

                var reportAttributes = reportIds.Any()
                    ? await reportAttributeRepository.FindAllAsync(ra => reportIds.Contains(ra.ReportID), cancellationToken)
                    : [];

                var attributesBySample = reportAttributes
                    .GroupBy(ra => ra.Report.SampleID)
                    .ToDictionary(g => g.Key, g => g.ToList());

                //build sample object with report and report attribute
                var detailedSamples = stageSamples.Select(s => new
                {
                    SampleName = s.Name,
                    IsInfected = s.InfectedSamples != null,
                    Reports = attributesBySample.GetValueOrDefault(s.ID, [])
                        .Select(ra => new
                        {
                            CreatedDate = ra.Report.Create_date,
                            ReferenceName = ra.Name,
                            Max = ra.Referent.ValueTo,
                            Min = ra.Referent.ValueFrom,
                            Actual = ra.Value,
                            Unit = ra.Referent.MeasurementUnit,
                        })
                        .OrderBy(r => r.CreatedDate)
                        .ToList()
                }).ToList();

                //add details sample into stage object
                stagesWithDetails.Add(new
                {
                    Step = stage.Step,
                    Name = stage.Name,
                    DateOfProcessing = stage.DateOfProcessing,
                    ElementInStage = stage.ElementInStages.Select(e => new
                    {
                        Elements = new
                        {
                            Name = e.Element.Name,
                            Description = e.Element.Description
                        }
                    }).ToList(),
                    Tasks = detailedTasks,
                    Samples = detailedSamples
                });

                allStageSamples.AddRange(stageSamples);
                allStageTasks.AddRange(stageTasks);
            }

            //get all technician involved in this stage
            var allTaskIds = allLinkeds
                .Where(l => l.TaskID != null)
                .Select(l => l.TaskID!)
                .Distinct()
                .ToList();

            var allAssigns = allTaskIds.Any()
                ? await taskAssignRepository.FindAllAsync(a => allTaskIds.Contains(a.TaskID), cancellationToken)
                : [];

            var allTechnicianIds = allAssigns.Select(a => a.TechnicianID).Distinct().ToList();
            var technicianNames = allTechnicianIds.Any()
                ? (await userRepository.FindAllAsync(u => allTechnicianIds.Contains(u.ID), cancellationToken))
                    .Select(u => u.Name)
                    .ToList()
                : [];

            //get seedling for experiment log
            var hybridizations = await hybridizationRepository.FindAllAsync(
                h => h.ExperimentLogID == request.ExperimentLogId, cancellationToken);

            var seedlingIds = hybridizations.Select(h => h.ParentID).ToList();
            var seedlings = seedlingIds.Any()
                ? await seedlingRepository.FindAllAsync(s => seedlingIds.Contains(s.ID), cancellationToken)
                : new List<Seedlings>();

            var totalTasks = allStageTasks.Count;
            var tasksOnTime = allStageTasks.Count(t => t.End_date <= DateTime.UtcNow);
            var tasksLate = allStageTasks.Count(t => t.End_date > DateTime.UtcNow);
            var tasksCancelled = allStageTasks.Count(t => t.Status == 5);

            var totalSamples = allStageSamples.Count;
            var samplesCompleted = allStageSamples.Count(s => s.Status == 0);
            var samplesSick = allStageSamples.Count(s => s.Status == 1);
            var samplesCancelled = allStageSamples.Count(s => s.Status == 2);

            var reportModel = new
            {
                experimentLog = new
                {
                    experimentLogData!.Name,
                    experimentLogData.Create_by,
                    Create_date = experimentLogData.Create_date,
                    End_date = totalDays,
                    Update_date = experimentLogData.Update_date,
                },
                technicianNames,
                seedlings = seedlings.Select(s => new
                {
                    s.LocalName,
                    s.ScientificName,
                    Characteristics = s.Characteristics.Select(c => new
                    {
                        SeedlingAttribute = new { c.SeedlingAttribute.Name },
                        c.Value
                    }).ToList()
                }),
                method = new
                {
                    method.Name,
                    Stages = stagesWithDetails
                },
                TotalTasks = totalTasks,
                TasksOnTime = tasksOnTime,
                TasksLate = tasksLate,
                TasksCancelled = tasksCancelled,
                TotalSamples = totalSamples,
                SamplesCompleted = samplesCompleted,
                SamplesSick = samplesSick,
                SamplesCancelled = samplesCancelled
            };

            var pdfBytes = await pdfReportGenerator.GenerateAsync(experimentLogData);
            return pdfBytes;
        }
    }
}
