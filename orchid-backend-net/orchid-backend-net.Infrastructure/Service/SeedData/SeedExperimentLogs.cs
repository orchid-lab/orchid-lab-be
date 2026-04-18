using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedExperimentLogs
    {
        private const string Flow1StartedExperimentName = "EL-MF1-STARTED";
        private const string Flow3SampleGenerationExperimentName = "EL-MF3-SAMPLE-GENERATION";
        private const string Flow6CompletedExperimentName = "EL-MF6-COMPLETED";

        public static async Task SeedAsync(DbContext context)
        {
            var invitroMethod = await context.Set<Methods>()
                .FirstOrDefaultAsync(m => EF.Functions.ILike(m.Name, "%Invitro%"));
            if (invitroMethod is null) return;

            var invitroMethodStages = await context.Set<MethodStages>()
                .Where(ms => ms.MethodId == invitroMethod.ID)
                .OrderBy(ms => ms.Order)
                .ToListAsync();
            if (invitroMethodStages.Count == 0) return;

            var maxInvitroStageOrder = invitroMethodStages.Max(ms => ms.Order);
            var stageOrder2 = invitroMethodStages.FirstOrDefault(s => s.Order == 2);
            var stageOrder3 = invitroMethodStages.FirstOrDefault(s => s.Order == 3);
            if (stageOrder2 is null || stageOrder3 is null)
                return;

            var sampleStageDefinitions = await context.Set<SampleStageDefinition>()
                .OrderBy(ss => ss.Order)
                .ToListAsync();
            if (sampleStageDefinitions.Count == 0)
                return;

            var sampleStageDefinitionIds = sampleStageDefinitions
                .Select(s => s.ID)
                .ToList();

            var sampleStageDefFirst = sampleStageDefinitions.First();
            var stageRequirements = await context.Set<StageRequirementDefinition>().ToListAsync();
            if (stageRequirements.Count == 0)
                return;

            var diseases = await context.Set<Disease>().ToListAsync();
            var healthyDisease = diseases.FirstOrDefault(d => d.Code == "healthy")
                ?? diseases.FirstOrDefault(d => d.Name.Contains("khỏe", StringComparison.OrdinalIgnoreCase));

            if (healthyDisease is null)
                return;

            var batches = await context.Set<Batches>()
                .OrderBy(b => b.ID)
                .Take(4)
                .ToListAsync();
            if (batches.Count < 4)
                return;

            var seedlingParent = await context.Set<Seedlings>().FirstOrDefaultAsync();
            if (seedlingParent is null)
                return;

            var technicianUser = await context.Set<Users>()
                .FirstOrDefaultAsync(u => u.RoleID == 3);
            var researcherUser = await context.Set<Users>()
                .FirstOrDefaultAsync(u => u.RoleID == 2);
            if (technicianUser is null || researcherUser is null)
                return;

            var now = DateTime.UtcNow;

            var flow1StartedLog = await context.Set<ExperimentLogs>()
                .FirstOrDefaultAsync(x => x.Name == Flow1StartedExperimentName);

            if (flow1StartedLog is null)
            {
                flow1StartedLog = new ExperimentLogs
                {
                    Name = Flow1StartedExperimentName,
                    MethodId = invitroMethod.ID,
                    Method = invitroMethod,
                    BatchId = batches[1].ID,
                    Batch = batches[1],
                    SeedlingParentId = seedlingParent.ID,
                    AssignedTo = technicianUser.ID,
                    ExpectedSampleCount = 8,
                    Status = ExperimentLogStatus.Created,
                    CurrentStageOrder = 0,
                    Objective = "Khởi tạo quy trình invitro và theo dõi task tự động theo method stage.",
                    Notes = "Technician đã bắt đầu thí nghiệm, hệ thống tự sinh task cho stage hiện tại.",
                    CreatedDate = now.AddDays(-10),
                    CreatedBy = researcherUser.ID,
                };

                flow1StartedLog.Start();
                flow1StartedLog.StartDate = DateOnly.FromDateTime(now.AddDays(-9));
                await context.Set<ExperimentLogs>().AddAsync(flow1StartedLog);
            }

            var flow3SampleGenerationLog = await context.Set<ExperimentLogs>()
                .FirstOrDefaultAsync(x => x.Name == Flow3SampleGenerationExperimentName);

            if (flow3SampleGenerationLog is null)
            {
                flow3SampleGenerationLog = new ExperimentLogs
                {
                    Name = Flow3SampleGenerationExperimentName,
                    MethodId = invitroMethod.ID,
                    Method = invitroMethod,
                    BatchId = batches[2].ID,
                    Batch = batches[2],
                    SeedlingParentId = seedlingParent.ID,
                    AssignedTo = technicianUser.ID,
                    ExpectedSampleCount = 6,
                    Status = ExperimentLogStatus.Created,
                    CurrentStageOrder = 0,
                    Objective = "Seed dữ liệu flow phát sinh sample ở stage đặc biệt và AI prediction.",
                    Notes = "Đã qua các stage đầu và đang ở stage có yêu cầu tạo mẫu.",
                    CreatedDate = now.AddDays(-18),
                    CreatedBy = researcherUser.ID,
                };

                flow3SampleGenerationLog.Start();
                flow3SampleGenerationLog.PendingToChangeStage();
                flow3SampleGenerationLog.MoveToNextStage(stageOrder2, maxInvitroStageOrder);
                flow3SampleGenerationLog.PendingToChangeStage();
                flow3SampleGenerationLog.MoveToNextStage(stageOrder3, maxInvitroStageOrder);
                flow3SampleGenerationLog.StartDate = DateOnly.FromDateTime(now.AddDays(-17));

                await context.Set<ExperimentLogs>().AddAsync(flow3SampleGenerationLog);
            }

            var flow6CompletedLog = await context.Set<ExperimentLogs>()
                .FirstOrDefaultAsync(x => x.Name == Flow6CompletedExperimentName);

            if (flow6CompletedLog is null)
            {
                flow6CompletedLog = new ExperimentLogs
                {
                    Name = Flow6CompletedExperimentName,
                    MethodId = invitroMethod.ID,
                    BatchId = batches[3].ID,
                    SeedlingParentId = seedlingParent.ID,
                    AssignedTo = technicianUser.ID,
                    ExpectedSampleCount = 4,
                    Status = ExperimentLogStatus.Completed,
                    CurrentStageOrder = maxInvitroStageOrder,
                    Objective = "Tổng hợp dữ liệu đầu-cuối cho báo cáo summary của một thí nghiệm đã hoàn tất.",
                    Conclusion = "Tỷ lệ sống đạt mục tiêu và đủ điều kiện kết thúc thí nghiệm.",
                    Issues = "Không ghi nhận vấn đề nghiêm trọng trong suốt chu kỳ theo dõi.",
                    Recommendations = "Tiếp tục duy trì quy trình hiện tại và chuẩn hóa checklist theo từng giai đoạn.",
                    StartDate = DateOnly.FromDateTime(now.AddDays(-45)),
                    EndDate = DateOnly.FromDateTime(now.AddDays(-5)),
                    Notes = "Dữ liệu dùng cho flow thống kê và xuất báo cáo tổng hợp.",
                    CreatedDate = now.AddDays(-46),
                    CreatedBy = researcherUser.ID,
                };
                await context.Set<ExperimentLogs>().AddAsync(flow6CompletedLog);
            }

            await context.SaveChangesAsync();

            // Seed samples for Flow 3
            var flow3Samples = await context.Set<Samples>()
                .Where(s => s.ExperimentLogId == flow3SampleGenerationLog.ID)
                .ToListAsync();

            if (flow3Samples.Count == 0)
            {
                flow3Samples = new List<Samples>
                {
                    new()
                    {
                        Name = "MF3-SAMPLE-01",
                        ExperimentLogId = flow3SampleGenerationLog.ID,
                        Notes = "Mẫu theo dõi chuẩn để approve monitoring",
                        InitialCondition = "Mẫu tươi, màu xanh nhạt, mô ổn định",
                        CreatedBy = technicianUser.ID,
                        CreatedDate = now.AddDays(-15)
                    },
                    new()
                    {
                        Name = "MF3-SAMPLE-02",
                        ExperimentLogId = flow3SampleGenerationLog.ID,
                        Notes = "Mẫu theo dõi ổn định để đối chiếu kết quả giữa các lần giám sát",
                        InitialCondition = "Mẫu hơi sậm màu, phát triển chậm",
                        CreatedBy = technicianUser.ID,
                        CreatedDate = now.AddDays(-15)
                    },
                    new()
                    {
                        Name = "MF3-SAMPLE-03",
                        ExperimentLogId = flow3SampleGenerationLog.ID,
                        Notes = "Mẫu theo dõi chờ researcher phê duyệt",
                        InitialCondition = "Mẫu ổn định, mô đồng đều",
                        CreatedBy = technicianUser.ID,
                        CreatedDate = now.AddDays(-14)
                    }
                };

                flow3Samples.ForEach(s => s.StartOnCreation(sampleStageDefFirst.ID));

                // Flow 5: one sample moved to next biological stage
                flow3Samples[0].CompleteCurrentStage(sampleStageDefinitionIds);

                await context.Set<Samples>().AddRangeAsync(flow3Samples);
                await context.SaveChangesAsync();
            }

            // Seed samples for Flow 6 (completed report)
            var flow6Samples = await context.Set<Samples>()
                .Where(s => s.ExperimentLogId == flow6CompletedLog.ID)
                .ToListAsync();

            if (flow6Samples.Count == 0)
            {
                flow6Samples = new List<Samples>
                {
                    new()
                    {
                        Name = "MF6-SAMPLE-01",
                        ExperimentLogId = flow6CompletedLog.ID,
                        Notes = "Mẫu hoàn thiện, đủ điều kiện chuyển seedling",
                        InitialCondition = "Mẫu khởi đầu khỏe mạnh",
                        CreatedBy = technicianUser.ID,
                        CreatedDate = now.AddDays(-40)
                    },
                    new()
                    {
                        Name = "MF6-SAMPLE-02",
                        ExperimentLogId = flow6CompletedLog.ID,
                        Notes = "Mẫu hoàn thiện dùng cho thống kê survival",
                        InitialCondition = "Mẫu khởi đầu khỏe mạnh",
                        CreatedBy = technicianUser.ID,
                        CreatedDate = now.AddDays(-40)
                    }
                };

                foreach (var sample in flow6Samples)
                {
                    sample.StartOnCreation(sampleStageDefFirst.ID);
                    sample.CompleteCurrentStage(sampleStageDefinitionIds);
                    sample.CompleteCurrentStage(sampleStageDefinitionIds);
                    sample.CompleteCurrentStage(sampleStageDefinitionIds);
                    sample.ConvertToSeedling();
                }

                await context.Set<Samples>().AddRangeAsync(flow6Samples);
                await context.SaveChangesAsync();
            }

            var hasMonitoringInFlow3 = await context.Set<MonitoringLogs>()
                .AnyAsync(m => flow3Samples.Select(s => s.ID).Contains(m.SampleStage.SampleId));

            if (!hasMonitoringInFlow3)
            {
                var stageBySampleId = await context.Set<SampleStage>()
                    .Where(ss => flow3Samples.Select(s => s.ID).Contains(ss.SampleId))
                    .ToListAsync();

                var sample1CurrentStage = stageBySampleId
                    .First(ss => ss.SampleId == flow3Samples[0].ID && ss.Status == SampleStatus.InProgressed);
                var sample2CurrentStage = stageBySampleId
                    .First(ss => ss.SampleId == flow3Samples[1].ID && ss.Status == SampleStatus.InProgressed);

                var analytic1 = new AnalyticResults
                {
                    Anthracnose = 0.00m,
                    BacterialWilt = 0.00m,
                    Blackrot = 0.00m,
                    Brownspots = 0.00m,
                    MoldBacterial = 0.00m,
                    MoldFungus = 0.00m,
                    SoftRot = 0.00m,
                    StemRot = 0.00m,
                    WitheredYellowRoot = 0.00m,
                    Healthy = 0.98m,
                    Oxidation = 0.00m,
                    Virus = 0.00m
                };

                var analytic2 = new AnalyticResults
                {
                    Anthracnose = 0.00m,
                    BacterialWilt = 0.00m,
                    Blackrot = 0.00m,
                    Brownspots = 0.00m,
                    MoldBacterial = 0.00m,
                    MoldFungus = 0.00m,
                    SoftRot = 0.00m,
                    StemRot = 0.00m,
                    WitheredYellowRoot = 0.00m,
                    Healthy = 0.99m,
                    Oxidation = 0.00m,
                    Virus = 0.00m
                };

                await context.Set<AnalyticResults>().AddRangeAsync(analytic1, analytic2);
                await context.SaveChangesAsync();

                var monitoringApproved = new MonitoringLogs
                {
                    Name = "MF4-REPORT-APPROVED",
                    UserId = technicianUser.ID,
                    AnalyticResultId = analytic1.ID,
                    SampleStageId = sample1CurrentStage.ID,
                    DiseaseId = healthyDisease.ID,
                    Notes = "Báo cáo giám sát đạt chuẩn, mẫu khỏe mạnh và được researcher duyệt.",
                    IsNewest = false,
                    CreatedDate = now.AddDays(-6),
                    CreatedBy = technicianUser.ID,
                };
                monitoringApproved.Created();
                monitoringApproved.SubmitForApproval(researcherUser.ID);
                monitoringApproved.Approve(researcherUser.ID);

                var monitoringWaiting = new MonitoringLogs
                {
                    Name = "MF4-REPORT-WAITING",
                    UserId = technicianUser.ID,
                    AnalyticResultId = analytic2.ID,
                    SampleStageId = sample2CurrentStage.ID,
                    DiseaseId = healthyDisease.ID,
                    Notes = "Báo cáo giám sát đạt chuẩn, đang chờ researcher phê duyệt.",
                    IsNewest = false,
                    CreatedDate = now.AddDays(-2),
                    CreatedBy = technicianUser.ID,
                };
                monitoringWaiting.Created();
                monitoringWaiting.SubmitForApproval(researcherUser.ID);

                foreach (var req in stageRequirements.Where(r => r.SampleStageDefinitionId == sample1CurrentStage.SampleStageDefinitionId))
                {
                    monitoringApproved.AddLogDetails(req.ID, req.ExpectedValue, true);
                }

                foreach (var req in stageRequirements.Where(r => r.SampleStageDefinitionId == sample2CurrentStage.SampleStageDefinitionId))
                {
                    var measured = req.MinValue.HasValue ? req.MinValue.Value : req.ExpectedValue;
                    monitoringWaiting.AddLogDetails(req.ID, measured, true);
                }

                await context.Set<MonitoringLogs>().AddRangeAsync(monitoringApproved, monitoringWaiting);
                await context.SaveChangesAsync();
                await context.Set<Imgs>().AddRangeAsync(
                    new Imgs
                    {
                        TargetId = monitoringApproved.ID,
                        Url = "https://example.com/img/mf4-monitoring-approved.jpg",
                        TargetType = ImageTargetType.MonitoringLog
                    },
                    new Imgs
                    {
                        TargetId = monitoringWaiting.ID,
                        Url = "https://example.com/img/mf4-monitoring-waiting.jpg",
                        TargetType = ImageTargetType.MonitoringLog
                    });

                await context.SaveChangesAsync();
            }

            // Seed monitoring logs for Flow 6 (all sample stages must have approved logs with full requirements)
            var flow6SampleIds = flow6Samples.Select(s => s.ID).ToList();
            var flow6SampleStages = await context.Set<SampleStage>()
                .Where(ss => flow6SampleIds.Contains(ss.SampleId))
                .ToListAsync();

            var existingApprovedStageIds = await context.Set<MonitoringLogs>()
                .Where(m => flow6SampleStages.Select(ss => ss.ID).Contains(m.SampleStageId)
                            && m.Status == MonitoringLogStatus.Approved)
                .Select(m => m.SampleStageId)
                .Distinct()
                .ToListAsync();

            var stagesToSeedMonitoring = flow6SampleStages
                .Where(ss => !existingApprovedStageIds.Contains(ss.ID))
                .ToList();

            if (stagesToSeedMonitoring.Count > 0)
            {
                var sampleNameById = flow6Samples.ToDictionary(s => s.ID, s => s.Name);
                var analytics = new List<AnalyticResults>();
                var monitoringLogs = new List<MonitoringLogs>();

                foreach (var stage in stagesToSeedMonitoring)
                {
                    var analytic = new AnalyticResults
                    {
                        Anthracnose = 0.00m,
                        BacterialWilt = 0.00m,
                        Blackrot = 0.00m,
                        Brownspots = 0.00m,
                        MoldBacterial = 0.00m,
                        MoldFungus = 0.00m,
                        SoftRot = 0.00m,
                        StemRot = 0.00m,
                        WitheredYellowRoot = 0.00m,
                        Healthy = 0.99m,
                        Oxidation = 0.00m,
                        Virus = 0.00m
                    };

                    var sampleName = sampleNameById.TryGetValue(stage.SampleId, out var name)
                        ? name
                        : stage.SampleId;

                    var monitoring = new MonitoringLogs
                    {
                        Name = $"MF6-REPORT-{sampleName}-STAGE-{stage.SampleStageDefinitionId}",
                        UserId = technicianUser.ID,
                        AnalyticResultId = analytic.ID,
                        SampleStageId = stage.ID,
                        DiseaseId = healthyDisease.ID,
                        Notes = "Báo cáo: toàn bộ chỉ số đạt ngưỡng quy cách của giai đoạn.",
                        IsNewest = false,
                        CreatedDate = now.AddDays(-12),
                        CreatedBy = technicianUser.ID,
                    };

                    monitoring.Created();
                    monitoring.SubmitForApproval(researcherUser.ID);
                    monitoring.Approve(researcherUser.ID);

                    foreach (var req in stageRequirements.Where(r => r.SampleStageDefinitionId == stage.SampleStageDefinitionId))
                    {
                        monitoring.AddLogDetails(req.ID, req.ExpectedValue, true);
                    }

                    analytics.Add(analytic);
                    monitoringLogs.Add(monitoring);
                }

                await context.Set<AnalyticResults>().AddRangeAsync(analytics);
                await context.Set<MonitoringLogs>().AddRangeAsync(monitoringLogs);
                await context.SaveChangesAsync();
            }
        }
    }
}