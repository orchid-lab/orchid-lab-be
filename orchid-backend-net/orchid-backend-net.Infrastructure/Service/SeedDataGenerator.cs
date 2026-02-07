using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Infrastructure.Service.SeedData;

namespace orchid_backend_net.Infrastructure.Service
{
    public static class SeedDataGenerator
    {
        public static async Task SeedAsync(DbContext context)
        {
            await SeedConfig.SeedAsync(context);

            await SeedRoles.SeedAsync(context);
            await SeedUsers.SeedAsync(context);

            await SeedChemicals.SeedAsync(context);
            await SeedMaterials.SeedAsync(context);

            await SeedSampleStageDefinition.SeedAsync(context);

            await SeedMethodStageDefinition.SeedAsync(context);
            await SeedMethod.SeedAsync(context);
            await SeedMethodStages.SeedAsync(context);
            await SeedStageChemicals.SeedAsync(context);
            await SeedStageMaterials.SeedAsync(context);

            await SeedCharacteristic.SeedAsync(context);   
            await SeedSeedlingAndSeedlingTraits.SeedAsync(context);
            
            await SeedSampleRequirementDefinition.SeedAsync(context);
            await SeedStageRequirement.SeedAsync(context);

            await SeedLabRooms.SeedAsync(context);
            await SeedBatches.SeedAsync(context);   
            await SeedTemplateTask.SeedAsync(context);
            await SeedDiseases.SeedAsync(context);

            await SeedExperimentLogs.SeedAsync(context);
        }
    }
}
