using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedExperimentLogs
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<ExperimentLogs>().AnyAsync())
            {
                var methods = await context.Set<Methods>().ToListAsync();
                var tissueBatches = await context.Set<TissueCultureBatches>().ToListAsync();

                var experimentLogs = new List<ExperimentLogs>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Initial Experiment Log",
                        CurrentStageID = "stage-a1",
                        MethodID = methods.FirstOrDefault()?.ID,
                        TissueCultureBatchID = tissueBatches.FirstOrDefault()?.ID,
                        Description = "Initial sterilization log",
                        Status = 1
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Subculutring log",
                        CurrentStageID = "stage-s1",
                        MethodID = methods.Skip(1).FirstOrDefault()?.ID,
                        TissueCultureBatchID = tissueBatches.Skip(1).FirstOrDefault()?.ID,
                        Description = "Subculturing log",
                        Status = 1
                    }
                };

                await context.Set<ExperimentLogs>().AddRangeAsync(experimentLogs);
                await context.SaveChangesAsync();
            }
        }
    }
}
