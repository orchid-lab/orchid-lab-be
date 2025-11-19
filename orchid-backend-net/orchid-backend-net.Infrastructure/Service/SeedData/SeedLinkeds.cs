using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedLinkeds
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Linkeds>().AnyAsync())
            {
                var samples = await context.Set<Samples>().ToListAsync();
                var tasks = await context.Set<Tasks>().ToListAsync();
                var experimentLogs = await context.Set<ExperimentLogs>().ToListAsync();

                var linkeds = new List<Linkeds>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        TaskID = tasks[0].ID,
                        SampleID = samples[0].ID,
                        ExperimentLogID = experimentLogs[0].ID,
                        ProcessStatus = 0
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        TaskID = tasks[1].ID,
                        SampleID = samples[1].ID,
                        ExperimentLogID = experimentLogs[1].ID,
                        ProcessStatus = 0
                    }
                };

                await context.Set<Linkeds>().AddRangeAsync(linkeds);
                await context.SaveChangesAsync();
            }
        }
    }
}
