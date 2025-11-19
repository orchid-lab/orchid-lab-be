using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedSamples
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Samples>().AnyAsync())
            {
                var samples = new List<Samples>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Sample A",
                        Description = "First orchid sample",
                        Dob = new DateOnly(2024, 5, 1),
                        Status = 0
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Sample B",
                        Description = "Second orchid sample",
                        Dob = new DateOnly(2024, 5, 10),
                        Status = 0
                    }
                };

                await context.Set<Samples>().AddRangeAsync(samples);
                await context.SaveChangesAsync();
            }
        }
    }
}
