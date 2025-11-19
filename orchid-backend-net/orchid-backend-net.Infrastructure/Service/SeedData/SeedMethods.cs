using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedMethods
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Methods>().AnyAsync())
            {
                var methods = new List<Methods>
                {
                    new() { ID = Guid.NewGuid().ToString(), Name = "Phương pháp lai vô tính", Description = "Nuôi cấy mô để nhân giống vô tính.", Type = "Clonal", Status = true, },
                    new() {  ID = Guid.NewGuid().ToString(), Name = "Phương pháp lai hữu tính", Description = "Kết hợp giữa các giống bố mẹ.", Type = "Sexual", Status = true, }
                };

                await context.Set<Methods>().AddRangeAsync(methods);
                await context.SaveChangesAsync();
            }
        }
    }
}
