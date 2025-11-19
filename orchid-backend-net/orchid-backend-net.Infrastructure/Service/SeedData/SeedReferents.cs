using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public class SeedReferents
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Referents>().AnyAsync())
            {
                var stages = await context.Set<Stages>().ToListAsync();

                var referents = new List<Referents>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        StageID = stages.First().ID,
                        Name = "Chiều cao mầm",
                        ValueFrom = 2.0m,
                        ValueTo = 5.0m,
                        MeasurementUnit = "cm",
                        Status = true
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        StageID = stages.Last().ID,
                        Name = "Số rễ",
                        ValueFrom = 3,
                        ValueTo = 7,
                        MeasurementUnit = "rễ",
                        Status = true
                    }
                };

                await context.Set<Referents>().AddRangeAsync(referents);
                await context.SaveChangesAsync();
            }
        }
    }
}
