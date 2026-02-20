using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedExperimentLogs
    {
        public static async Task SeedAsync(DbContext context)
        {
            // Avoid reseeding if we already have experiment logs
            if (await context.Set<ExperimentLogs>().AnyAsync())
                return;

            // Resolve required references
            var invitroMethod = await context.Set<Methods>()
                .FirstOrDefaultAsync(m => EF.Functions.ILike(m.Name, "%Invitro%"));
            if (invitroMethod is null) return;

            var methodStages = await context.Set<MethodStages>()
                .Where(ms => ms.MethodId == invitroMethod.ID)
                .OrderBy(ms => ms.Order)
                .ToListAsync();
            if (methodStages.Count == 0) return;

            var stageGeneratesSamples = methodStages.FirstOrDefault(ms => ms.IsSampleGenerated);
            var maxStageOrder = methodStages.Max(ms => ms.Order);

            // Any batch and lab room already seeded
            var batch = await context.Set<Batches>().FirstOrDefaultAsync();
            var seedlingParent = await context.Set<Seedlings>().FirstOrDefaultAsync();
            var technicianUser = await context.Set<Users>()
                .FirstOrDefaultAsync(u => EF.Functions.ILike(u.Email, "%tech%") || EF.Functions.ILike(u.Name, "%tech%"));
            var researcherUser = await context.Set<Users>()
                .FirstOrDefaultAsync(u => EF.Functions.ILike(u.Email, "%research%") || EF.Functions.ILike(u.Name, "%research%"));

            if (batch is null || seedlingParent is null || technicianUser is null)
                return;

            // 1) Created (not started) experiment log with Invitro method
            var createdLog = new ExperimentLogs
            {
                ID = Guid.NewGuid().ToString(),
                Name = "EL-INV-CREATED",
                MethodId = invitroMethod.ID,
                BatchId = batch.ID,
                SeedlingParentId = seedlingParent.ID,
                AssignedTo = technicianUser.ID,
                ExpectedSampleCount = 12,
                Status = ExperimentLogStatus.Created,
                CurrentStageOrder = 0,
                Notes = "Experiment created, waiting to start.",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = researcherUser?.ID ?? "System"
            };

            // 2) In progress, pending stage that generates samples (to test notifications)
            // We will set to WaitingForChangeStage at the stage that is right before the sample-generating stage, 
            // so moving next will raise ExperimentLogSampleGenerationRequired domain event.
            var beforeSampleStageOrder = stageGeneratesSamples is not null
                ? Math.Max(stageGeneratesSamples.Order - 1, 1)
                : 1;

            var pendingToGenerateSamplesLog = new ExperimentLogs
            {
                ID = Guid.NewGuid().ToString(),
                Name = "EL-INV-PENDING-SAMPLE",
                MethodId = invitroMethod.ID,
                BatchId = batch.ID,
                SeedlingParentId = seedlingParent.ID,
                AssignedTo = technicianUser.ID,
                ExpectedSampleCount = 6,
                Status = ExperimentLogStatus.WaitingForChangeStage,
                CurrentStageOrder = beforeSampleStageOrder,
                Notes = "Ready to move to the next stage that generates samples.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)),
                CreatedDate = DateTime.UtcNow.AddDays(-8),
                CreatedBy = researcherUser?.ID ?? "System"
            };

            // 3) Experiment log with small number of samples and sample stages + monitoring logs
            var samplesLog = new ExperimentLogs
            {
                ID = Guid.NewGuid().ToString(),
                Name = "EL-INV-WITH-SAMPLES",
                MethodId = invitroMethod.ID,
                BatchId = batch.ID,
                SeedlingParentId = seedlingParent.ID,
                AssignedTo = technicianUser.ID,
                ExpectedSampleCount = 3,
                Status = ExperimentLogStatus.InProgress,
                CurrentStageOrder = stageGeneratesSamples?.Order ?? 1,
                Notes = "Contains a few samples for stage movement testing.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14)),
                CreatedDate = DateTime.UtcNow.AddDays(-15),
                CreatedBy = researcherUser?.ID ?? "System"
            };

            await context.Set<ExperimentLogs>().AddRangeAsync(createdLog, pendingToGenerateSamplesLog, samplesLog);
            await context.SaveChangesAsync();

            // Seed samples for the third experiment log
            var sampleStageDefs = await context.Set<SampleStageDefinition>()
                .OrderBy(ssd => ssd.ID)
                .ToListAsync();
            if (sampleStageDefs.Count == 0)
                return;

            var sampleStageDefFirst = sampleStageDefs.First();
            var sampleStageDefSecond = sampleStageDefs.Count > 1 ? sampleStageDefs[1] : sampleStageDefFirst;

            var samples = new List<Samples>();
            for (int i = 1; i <= 3; i++)
            {
                samples.Add(new Samples
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = $"SAMPLE-{i}",
                    ExperimentLogId = samplesLog.ID,
                    Notes = i == 1 ? "Healthy sample" : i == 2 ? "Slightly weak" : "Under observation",
                    ExecutionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
                });
            }
            await context.Set<Samples>().AddRangeAsync(samples);
            await context.SaveChangesAsync();

            // Assign initial sample stages
            var sampleStages = new List<SampleStage>();
            foreach (var s in samples)
            {
                sampleStages.Add(new SampleStage
                {
                    ID = Guid.NewGuid().ToString(),
                    SampleId = s.ID,
                    SampleStageDefinitionId = sampleStageDefFirst.ID,
                    StartedAt = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9)),
                    CompletedAt = null,
                    Status = SampleStatus.InProgressed
                });
            }
            // Move one sample to second stage to test stage movement
            sampleStages.Add(new SampleStage
            {
                ID = Guid.NewGuid().ToString(),
                SampleId = samples[0].ID,
                SampleStageDefinitionId = sampleStageDefSecond.ID,
                StartedAt = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3)),
                CompletedAt = null,
                Status = SampleStatus.InProgressed
            });

            await context.Set<SampleStage>().AddRangeAsync(sampleStages);
            await context.SaveChangesAsync();

            // Seed diseases catalog if empty (optional safeguard)
            if (!await context.Set<Disease>().AnyAsync())
            {
                await context.Set<Disease>().AddRangeAsync(
                    new Disease { Name = "Bacterial Wilt", Description = "Vi khuẩn héo rũ" },
                    new Disease { Name = "Fungal Infection", Description = "Nhiễm nấm" }
                );
                await context.SaveChangesAsync();
            }

            var diseases = await context.Set<Disease>().ToListAsync();
            var bacterialWilt = diseases.FirstOrDefault(d => d.Name.Contains("Bacterial", StringComparison.OrdinalIgnoreCase));
            var fungalInfection = diseases.FirstOrDefault(d => d.Name.Contains("Fungal", StringComparison.OrdinalIgnoreCase));

            // Seed monitoring logs with analytics and details to test notifications and newest flags
            var analytic1 = new AnalyticResults
            {
                ID = Guid.NewGuid().ToString(),
                Anthracnose = 0.02m,
                BacterialWilt = 0.15m,
                Blackrot = 0.00m,
                Brownspots = 0.01m,
                MoldBacterial = 0.08m,
                MoldFungus = 0.04m,
                SoftRot = 0.00m,
                StemRot = 0.00m,
                WitheredYellowRoot = 0.00m,
                Healthy = 0.70m,
                Oxidation = 0.00m,
                Virus = 0.00m
            };
            var analytic2 = new AnalyticResults
            {
                ID = Guid.NewGuid().ToString(),
                Anthracnose = 0.00m,
                BacterialWilt = 0.00m,
                Blackrot = 0.05m,
                Brownspots = 0.02m,
                MoldBacterial = 0.00m,
                MoldFungus = 0.10m,
                SoftRot = 0.00m,
                StemRot = 0.00m,
                WitheredYellowRoot = 0.00m,
                Healthy = 0.78m,
                Oxidation = 0.00m,
                Virus = 0.00m
            };

            await context.Set<AnalyticResults>().AddRangeAsync(analytic1, analytic2);
            await context.SaveChangesAsync();

            var s1Stage = sampleStages.First(ss => ss.SampleId == samples[0].ID && ss.SampleStageDefinitionId == sampleStageDefSecond.ID);
            var s2Stage = sampleStages.First(ss => ss.SampleId == samples[1].ID && ss.SampleStageDefinitionId == sampleStageDefFirst.ID);

            var monitoring1 = new MonitoringLogs
            {
                ID = Guid.NewGuid().ToString(),
                UserId = technicianUser.ID,
                AnalyticResultId = analytic1.ID,
                SampleStageId = s1Stage.ID,
                DiseaseId = bacterialWilt?.ID,
                Notes = "Mẫu có dấu hiệu nhẹ của bacterial wilt.",
                Status = MonitoringLogStatus.Approved,
                IsNewest = true,
                CreatedDate = DateTime.UtcNow.AddDays(-2),
                CreatedBy = technicianUser.ID
            };

            var monitoring2 = new MonitoringLogs
            {
                ID = Guid.NewGuid().ToString(),
                UserId = technicianUser.ID,
                AnalyticResultId = analytic2.ID,
                SampleStageId = s2Stage.ID,
                DiseaseId = fungalInfection?.ID,
                Notes = "Phát hiện nấm mức độ thấp.",
                Status = MonitoringLogStatus.WaitingForApproval,
                IsNewest = true,
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                CreatedBy = technicianUser.ID
            };

            await context.Set<MonitoringLogs>().AddRangeAsync(monitoring1, monitoring2);
            await context.SaveChangesAsync();

            // Monitoring details linked to stage requirements
            var stageRequirements = await context.Set<StageRequirementDefinition>().ToListAsync();
            var detailPayloads = new List<LogDetails>();

            if (stageRequirements.Count > 0)
            {
                var sr1 = stageRequirements[0];
                var sr2 = stageRequirements.Count > 1 ? stageRequirements[1] : sr1;

                detailPayloads.Add(new LogDetails
                {
                    ID = Guid.NewGuid().ToString(),
                    MonitoringLogsId = monitoring1.ID,
                    StageRequirementDefinitionId = sr1.ID,
                    MeasuredValue = sr1.ExpectedValue,
                    IsMatch = true
                });
                detailPayloads.Add(new LogDetails
                {
                    ID = Guid.NewGuid().ToString(),
                    MonitoringLogsId = monitoring2.ID,
                    StageRequirementDefinitionId = sr2.ID,
                    MeasuredValue = sr2.MinValue.HasValue ? sr2.MinValue.Value : sr2.ExpectedValue,
                    IsMatch = true
                });
            }

            if (detailPayloads.Count > 0)
            {
                await context.Set<LogDetails>().AddRangeAsync(detailPayloads);
                await context.SaveChangesAsync();
            }

            // Optional images for monitoring logs
            await context.Set<Imgs>().AddRangeAsync(
                new Imgs { ID = Guid.NewGuid().ToString(), TargetId = monitoring1.ID, Url = "https://example.com/img/monitoring1.jpg", TargetType = ImageTargetType.MonitoringLog },
                new Imgs { ID = Guid.NewGuid().ToString(), TargetId = monitoring2.ID, Url = "https://example.com/img/monitoring2.jpg", TargetType = ImageTargetType.MonitoringLog }
            );
            await context.SaveChangesAsync();
        }
    }
}