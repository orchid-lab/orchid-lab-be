using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedTaskTemplate
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<TaskTemplates>().AnyAsync())
            {
                var taskTemplates = new List<TaskTemplates>
                {
                    new() { ID = "task-a1", Name = "Chuẩn bị môi trường", StageID = "stage-a1", Description = "Pha chế dung dịch nuôi cấy", Status = true },
                    new() { ID = "task-a2", Name = "Tiệt trùng mẫu", StageID = "stage-a1", Description = "Tiệt trùng trước khi đưa vào môi trường.", Status = true },
                    new() { ID = "task-a3", Name = "Theo dõi chuyển mẫu", StageID = "stage-a2", Description = "Theo dõi mẫu sau khi chuyển môi trường mới.", Status = true },
                    new() { ID = "task-s1", Name = "Ghi nhận mẫu lai", StageID = "stage-s1", Description = "Ghi nhận mẫu chuẩn bị lai.", Status = true },
                    new() { ID = "task-s2", Name = "Theo dõi sinh trưởng", StageID = "stage-s2", Description = "Quan sát sinh trưởng của mẫu.", Status = true }
                };

                await context.Set<TaskTemplates>().AddRangeAsync(taskTemplates);
                await context.SaveChangesAsync();
            }
        }
    }
}
