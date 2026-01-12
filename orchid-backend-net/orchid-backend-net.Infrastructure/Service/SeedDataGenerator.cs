using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Infrastructure.Service.SeedData;

namespace orchid_backend_net.Infrastructure.Service
{
    public static class SeedDataGenerator
    {
        public static async Task SeedAsync(DbContext context)
        {
            await SeedRoles.SeedAsync(context);
            await SeedUsers.SeedAsync(context);

            await SeedChemicals.SeedAsync(context);
            await SeedMaterials.SeedAsync(context);

            await SeedSampleStageDefinition.SeedAsync(context);
        }
    }
}
