using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedSeedlings
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!context.Set<Seedlings>().Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    var name = $"Seedling-{i}";

                    var exists = await context.Set<Seedlings>().AnyAsync(x => x.LocalName == name);
                    if (!exists)
                    {
                        context.Set<Seedlings>().Add(new Seedlings
                        {
                            LocalName = name,
                            ScientificName = name,
                            Description = $"Description for {name}",
                            Parent1 = $"Parent1-{i}",
                            Parent2 = $"Parent2-{i}",
                            Dob = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i * 30)),
                            Create_date = DateTime.UtcNow,
                            Create_by = "system",
                        });
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
