using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedStages
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Stages>().AnyAsync())
            {
                var sterilization = await context.Set<Methods>().FirstOrDefaultAsync(m => m.Name == "Phương pháp lai vô tính");
                var subculturing = await context.Set<Methods>().FirstOrDefaultAsync(m => m.Name == "Phương pháp lai hữu tính");

                var stages = new List<Stages>
                {
                    new() { ID = "stage-a1", Name = "Chuẩn bị mẫu", Description = "Chuẩn bị môi trường nuôi cấy.", Step = 1, MethodID = sterilization.ID, DateOfProcessing = 3, Status = true },
                    new() { ID = "stage-a2", Name = "Chuyển mẫu", Description = "Đưa vào môi trường mới.", Step = 2, MethodID = sterilization.ID, DateOfProcessing = 5, Status = true },
                    new() { ID = "stage-s1", Name = "Chuẩn bị thụ phấn", Description = "Đánh dấu mẫu, chuẩn bị phấn hoa.", Step = 1, MethodID = subculturing.ID, DateOfProcessing = 2, Status = true },
                    new() { ID = "stage-s2", Name = "Theo dõi phát triển", Description = "Quan sát sự phát triển sau lai hữu tính.", Step = 2, MethodID = subculturing.ID, DateOfProcessing = 7, Status = true }
                };

                await context.Set<Stages>().AddRangeAsync(stages);
                await context.SaveChangesAsync();
            }
        }
    }
}
