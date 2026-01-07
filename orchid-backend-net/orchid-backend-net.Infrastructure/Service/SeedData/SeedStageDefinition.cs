using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedStageDefinition
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<StageDefinition>().AnyAsync())
            {
                var stages = new List<StageDefinition>
            {
                new()
                {
                    ID = 1,
                    Name = "Giai đoạn mầm",
                    Description = "Giai đoạn khởi đầu, mẫu bắt đầu nảy mầm và hình thành mô sơ cấp"
                },
                new()
                {
                    ID = 2,
                    Name = "Giai đoạn chồi",
                    Description = "Giai đoạn phát triển chồi, tăng sinh và nhân nhanh"
                },
                new()
                {
                    ID = 3,
                    Name = "Giai đoạn cây hoàn chỉnh",
                    Description = "Giai đoạn hình thành cây con hoàn chỉnh, sẵn sàng thuần hóa"
                }
            };

                await context.Set<StageDefinition>().AddRangeAsync(stages);
                await context.SaveChangesAsync();
            }
        }
    }
}
