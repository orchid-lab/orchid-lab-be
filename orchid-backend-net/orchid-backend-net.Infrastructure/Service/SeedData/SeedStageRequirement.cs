using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedStageRequirement
    {
        public static async Task SeedAsync(DbContext context)
        {
            var stages = await context.Set<SampleStageDefinition>().ToListAsync();
            var requirements = await context.Set<SamplesRequirementsDefinition>().ToListAsync();
            int Stage(string name)
            => stages.First(s => s.Name == name).ID;

            string Req(string name)
                => requirements.First(r => r.Name == name).ID;
            if (!await context.Set<StageRequirementDefinition>().AnyAsync())
            {
                var data = new List<StageRequirementDefinition>()
                {
                    // =========================
                    // GIAI ĐOẠN MẦM
                    // =========================
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn mầm"),
                        SampleRequirementDefinitionId = Req("Tỷ lệ nảy mầm"),
                        ExpectedValue = 70,
                        MinValue = 50,
                        MaxValue = 100
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn mầm"),
                        SampleRequirementDefinitionId = Req("Đường kính protocorm"),
                        ExpectedValue = 2.0m,
                        MinValue = 1.0m,
                        MaxValue = 3.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn mầm"),
                        SampleRequirementDefinitionId = Req("Thời gian nảy mầm"),
                        ExpectedValue = 21,
                        MinValue = 14,
                        MaxValue = 30
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn mầm"),
                        SampleRequirementDefinitionId = Req("Tỷ lệ sống protocorm"),
                        ExpectedValue = 85,
                        MinValue = 70,
                        MaxValue = 100
                    },

                    // =========================
                    // GIAI ĐOẠN CHỒI
                    // =========================
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Số PLB hình thành"),
                        ExpectedValue = 5,
                        MinValue = 2,
                        MaxValue = 10
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Đường kính PLB"),
                        ExpectedValue = 4.0m,
                        MinValue = 3.0m,
                        MaxValue = 6.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Thời gian hình thành PLB"),
                        ExpectedValue = 30,
                        MinValue = 21,
                        MaxValue = 45
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Số chồi"),
                        ExpectedValue = 3,
                        MinValue = 1,
                        MaxValue = 6
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Chiều cao chồi"),
                        ExpectedValue = 2.5m,
                        MinValue = 1.5m,
                        MaxValue = 4.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Chiều dài thân giả"),
                        ExpectedValue = 1.5m,
                        MinValue = 1.0m,
                        MaxValue = 3.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn chồi"),
                        SampleRequirementDefinitionId = Req("Tỷ lệ sống chồi"),
                        ExpectedValue = 90,
                        MinValue = 80,
                        MaxValue = 100
                    },

                    // =========================
                    // GIAI ĐOẠN CÂY HOÀN CHỈNH
                    // =========================
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Số lá"),
                        ExpectedValue = 4,
                        MinValue = 3,
                        MaxValue = 6
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Chiều dài lá"),
                        ExpectedValue = 6.0m,
                        MinValue = 4.0m,
                        MaxValue = 8.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Chiều rộng lá"),
                        ExpectedValue = 2.5m,
                        MinValue = 1.8m,
                        MaxValue = 3.5m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Độ dày lá"),
                        ExpectedValue = 1.2m,
                        MinValue = 0.8m,
                        MaxValue = 1.8m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Số rễ"),
                        ExpectedValue = 3,
                        MinValue = 2,
                        MaxValue = 6
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Chiều dài rễ"),
                        ExpectedValue = 4.0m,
                        MinValue = 2.5m,
                        MaxValue = 6.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Chiều cao cây con"),
                        ExpectedValue = 7.0m,
                        MinValue = 5.0m,
                        MaxValue = 10.0m
                    },
                    new()
                    {
                        SampleStageDefinitionId = Stage("Giai đoạn cây hoàn chỉnh"),
                        SampleRequirementDefinitionId = Req("Tỷ lệ sống cây con"),
                        ExpectedValue = 95,
                        MinValue = 85,
                        MaxValue = 100
                    }
                };
                await context.Set<StageRequirementDefinition>().AddRangeAsync(data);
                await context.SaveChangesAsync();
            }
        }
    }
}
