using Microsoft.EntityFrameworkCore;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedConfig
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Domain.Entities.Config>().AnyAsync())
            {
                var configs = new List<Domain.Entities.Config>
                {
                    new() {
                        ConfigName = "Maximum Batch Sample Count",
                        Key = "MaxSampleCountPerExperimentLog",
                        Value = 15
                    },
                };
                await context.Set<Domain.Entities.Config>().AddRangeAsync(configs);
                await context.SaveChangesAsync();
            }
        }
    }
}
