using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Service.SeedData;

namespace orchid_backend_net.Infrastructure.Service
{
    public static class SeedDataGenerator
    {
        public static async Task SeedAsync(DbContext context)
        {
            await SeedRoles.SeedAsync(context);
            await SeedUsers.SeedAsync(context);
            await SeedDisease.SeedAsync(context);

            await SeedLabRooms.SeedAsync(context);
            await SeedMethods.SeedAsync(context);
            await SeedStages.SeedAsync(context);
            await SeedReferents.SeedAsync(context);
            await SeedElements.SeedAsync(context);
            await SeedSamples.SeedAsync(context);
            await SeedTissueCultureBatches.SeedAsync(context);
            await SeedTasks.SeedAsync(context);
            await SeedTaskAssigns.SeedAsync(context);
            await SeedExperimentLogs.SeedAsync(context);
            await SeedLinkeds.SeedAsync(context);

            await SeedSeedlings.SeedAsync(context);
            await SeedSeedlingAttributes.SeedAsync(context);
            await SeedCharacteristics.SeedAsync(context);

            await SeedReports.SeedAsync(context);
            await SeedReportAttributes.SeedAsync(context);

            await SeedTaskTemplate.SeedAsync(context);
            await SeedTaskTemplateDetail.SeedAsync(context);
        }
    }
}
