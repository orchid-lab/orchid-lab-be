using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedStageRequirement
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<StageRequirementDefinition>().AnyAsync())
            {
                var stages = await context.Set<SampleStageDefinition>().ToListAsync();
                var requirements = await context.Set<SamplesRequirementsDefinition>().ToListAsync();

                var stageRequirements = new List<StageRequirementDefinition>();

                foreach (var stage in stages)
                {
                    foreach (var req in requirements)
                    {
                        // Quy tắc: tất cả các characteristic đo được ở cây con đều áp dụng cho stage
                        stageRequirements.Add(new StageRequirementDefinition
                        {
                            SampleStageDefinitionId = stage.ID,
                            SampleRequirementDefinitionId = req.ID,
                            ExpectedValue = req.DefaultExpectedValue
                        });
                    }
                }

                await context.Set<StageRequirementDefinition>().AddRangeAsync(stageRequirements);
                await context.SaveChangesAsync();
            }
        }
    }
}
