using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedSampleStageDefinition
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<SampleStageDefinition>().AnyAsync())
            {
                var stages = new List<SampleStageDefinition>
            {
                new()
                {
                    Order = 1,
                    Name = "Giai đoạn mầm",
                    Description = "Giai đoạn khởi đầu, mẫu bắt đầu nảy mầm và hình thành mô sơ cấp"
                },
                new()
                {
                    Order = 2,
                    Name = "Giai đoạn chồi",
                    Description = "Giai đoạn phát triển chồi, tăng sinh và nhân nhanh"
                },
                new()
                {
                    Order = 3,
                    Name = "Giai đoạn cây hoàn chỉnh",
                    Description = "Giai đoạn hình thành cây con hoàn chỉnh, sẵn sàng thuần hóa"
                }
            };

                await context.Set<SampleStageDefinition>().AddRangeAsync(stages);
                await context.SaveChangesAsync();
            }
        }
    }
}
