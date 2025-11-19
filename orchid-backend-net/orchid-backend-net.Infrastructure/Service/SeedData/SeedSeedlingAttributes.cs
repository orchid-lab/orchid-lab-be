using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedSeedlingAttributes
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!context.Set<SeedlingAttributes>().Any())
            {
                var attributeNames = new[]
                {
                    "Height", "Width", "Color", "LeafCount", "RootLength",
                    "StemThickness", "FlowerSize", "GrowthRate", "ChlorophyllLevel", "DiseaseResistance"
                };

                foreach (var attribute in attributeNames)
                {
                    var exist = await context.Set<SeedlingAttributes>().AnyAsync(a => a.Name == attribute);
                    if (!exist)
                    {
                        var newAttribute = new SeedlingAttributes
                        {
                            ID = Guid.NewGuid().ToString(),
                            Name = attribute,
                            Description = $"{attribute} of the orchid seedling",
                            Status = true
                        };
                        await context.Set<SeedlingAttributes>().AddAsync(newAttribute);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
