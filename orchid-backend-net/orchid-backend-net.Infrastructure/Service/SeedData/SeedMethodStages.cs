using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Common.Const;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedMethodStages
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<MethodStages>().AnyAsync()) return;

            var methods = await context.Set<Methods>().ToListAsync();
            var stageDefs = await context.Set<MethodStageDefinition>().ToListAsync();

            int M(string name) =>
                methods.First(x => x.Name.StartsWith(name)).ID;

            int S(string name) =>
                stageDefs.First(x => x.Name.StartsWith(name)).ID;

            var data = new List<MethodStages>
        {
            // ===== In vitro =====
            new() {
                MethodId = M(MethodNames.INVITRO),
                MethodStageDefinitionId = S(StageNames.SAMPLING),
                Order = 1,
                DurationsDays = 1
            },
            new() {
                MethodId = M(MethodNames.INVITRO),
                MethodStageDefinitionId = S(StageNames.STERILIZATION),
                Order = 2,
                DurationsDays = 1
            },
            new() {
                MethodId = M(MethodNames.INVITRO),
                MethodStageDefinitionId = S(StageNames.INITIATION),
                Order = 3,
                DurationsDays = 14,
                IsSampleGenerated = true
            },
            new() {
                MethodId = M(MethodNames.INVITRO),
                MethodStageDefinitionId = S(StageNames.MULTIPLICATION),
                Order = 4,
                DurationsDays = 30
            },
            new() {
                MethodId = M(MethodNames.INVITRO),
                MethodStageDefinitionId = S(StageNames.ROOTING),
                Order = 5,
                DurationsDays = 21
            },
            new() {
                MethodId = M(MethodNames.INVITRO),
                MethodStageDefinitionId = S(StageNames.ACCLIMATIZATION),
                Order = 6,
                DurationsDays = 30
            },

            // ===== TCL =====
            new() {
                MethodId = M(MethodNames.TCL),
                MethodStageDefinitionId = S(StageNames.SAMPLING),
                Order = 1,
                DurationsDays = 1
            },
            new() {
                MethodId = M(MethodNames.TCL),
                MethodStageDefinitionId = S(StageNames.STERILIZATION),
                Order = 2,
                DurationsDays = 1
            },
            new() {
                MethodId = M(MethodNames.TCL),
                MethodStageDefinitionId = S(StageNames.INITIATION),
                Order = 3,
                DurationsDays = 10,
                IsSampleGenerated = true
            },
            new() {
                MethodId = M(MethodNames.TCL),
                MethodStageDefinitionId = S(StageNames.MULTIPLICATION),
                Order = 4,
                DurationsDays = 25
            },
            new() {
                MethodId = M(MethodNames.TCL),
                MethodStageDefinitionId = S(StageNames.ROOTING),
                Order = 5,
                DurationsDays = 20
            },
            new() {
                MethodId = M(MethodNames.TCL),
                MethodStageDefinitionId = S(StageNames.ACCLIMATIZATION),
                Order = 6,
                DurationsDays = 30
            },

            // ===== DIVISION =====
            new() {
                MethodId = M(MethodNames.DIVISION),
                MethodStageDefinitionId = S(StageNames.SAMPLING),
                Order = 1,
                DurationsDays = 1,
                IsSampleGenerated = true
            },
            new() {
                MethodId = M(MethodNames.DIVISION),
                MethodStageDefinitionId = S(StageNames.ROOTING),
                Order = 2,
                DurationsDays = 30
            },
            new() {
                MethodId = M(MethodNames.DIVISION),
                MethodStageDefinitionId = S(StageNames.ACCLIMATIZATION),
                Order = 3,
                DurationsDays = 30
            },

            // ===== Thân giả =====
            new() {
                MethodId = M(MethodNames.PSEUDOBULB),
                MethodStageDefinitionId = S(StageNames.SAMPLING),
                Order = 1,
                DurationsDays = 1
            },
            new() {
                MethodId = M(MethodNames.PSEUDOBULB),
                MethodStageDefinitionId = S(StageNames.INITIATION),
                Order = 2,
                DurationsDays = 21,
                IsSampleGenerated = true
            },
            new() {
                MethodId = M(MethodNames.PSEUDOBULB),
                MethodStageDefinitionId = S(StageNames.ROOTING),
                Order = 3,
                DurationsDays = 30
            },
            new() {
                MethodId = M(MethodNames.PSEUDOBULB),
                MethodStageDefinitionId = S(StageNames.ACCLIMATIZATION),
                Order = 4,
                DurationsDays = 30
            },

            // ===== Tách nhánh =====
            new() {
                MethodId = M(MethodNames.CUTTING),
                MethodStageDefinitionId = S(StageNames.SAMPLING),
                Order = 1,
                DurationsDays = 1,
                IsSampleGenerated = true
            },
            new() {
                MethodId = M(MethodNames.CUTTING),
                MethodStageDefinitionId = S(StageNames.ROOTING),
                Order = 2,
                DurationsDays = 30
            },
            new() {
                MethodId = M(MethodNames.CUTTING),
                MethodStageDefinitionId = S(StageNames.ACCLIMATIZATION),
                Order = 3,
                DurationsDays = 30
            }
        };

            await context.Set<MethodStages>().AddRangeAsync(data);
            await context.SaveChangesAsync();
        }
    }


}
