using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedCharacteristics
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!context.Set<Characteristics>().Any())
            {
                var seedlings = await context.Set<Seedlings>().Take(10).ToListAsync();
                var attributes = await context.Set<SeedlingAttributes>().Take(10).ToListAsync();
                var random = new Random();

                foreach (var seedling in seedlings)
                {
                    foreach (var attr in attributes.Take(3)) //randomly select 3 attributes for each seedling
                    {
                        var exists = await context.Set<Characteristics>().AnyAsync(x =>
                            x.SeedlingID == seedling.ID && x.SeedlingAttributeID == attr.ID);

                        if (!exists)
                        {
                            context.Set<Characteristics>().Add(new Characteristics
                            {
                                SeedlingID = seedling.ID,
                                SeedlingAttributeID = attr.ID,
                                Value = (decimal)(random.NextDouble() * 100),
                                Status = true
                            });
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
