using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedTaskTemplateDetail
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<TaskTemplateDetails>().AnyAsync())
            {
                var details = new List<TaskTemplateDetails>
                {
                    new() { ID = Guid.NewGuid().ToString(), TaskTemplateID = "task-a1", Element = "Nước cất", Name = "Dung tích cần thiết", Description = "Sử dụng nước cất để pha môi trường", ExpectedValue = 500, Unit = "ml", IsRequired = true, Status = true },
                    new() { ID = Guid.NewGuid().ToString(), TaskTemplateID = "task-a2", Element = "Dung dịch cồn", Name = "Nồng độ", Description = "Cồn tiệt trùng mẫu", ExpectedValue = 70, Unit = "%", IsRequired = true, Status = true },
                    new() { ID = Guid.NewGuid().ToString(), TaskTemplateID = "task-s1", Element = "Số lượng hạt phấn", Name = "Số lượng", Description = "Đếm số lượng hạt phấn sử dụng.", ExpectedValue = 20, Unit = "hạt", IsRequired = true, Status = true },
                    new() { ID = Guid.NewGuid().ToString(), TaskTemplateID = "task-s2", Element = "Độ cao cây", Name = "Chiều cao kỳ vọng", Description = "Theo dõi chiều cao mẫu cây sau thụ phấn.", ExpectedValue = 15, Unit = "cm", IsRequired = false, Status = true }
                };

                await context.Set<TaskTemplateDetails>().AddRangeAsync(details);
                await context.SaveChangesAsync();
            }
        }
    }
}
