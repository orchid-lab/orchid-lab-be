using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedLabRooms
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<LabRooms>().AnyAsync())
                return;

            // ===== LAB ROOMS =====
            var tissueCultureLab = new LabRooms
            {
                Name = "Phòng cấy mô",
                Description = "Thực hiện khử trùng và cấy mô ban đầu",
                Status = 1
            };

            var multiplicationLab = new LabRooms
            {
                Name = "Phòng nhân chồi",
                Description = "Nhân nhanh chồi in-vitro",
                Status = 1
            };

            var rootingLab = new LabRooms
            {
                Name = "Phòng ra rễ",
                Description = "Kích thích ra rễ trước khi ra vườn ươm",
                Status = 1
            };

            var acclimatizationLab = new LabRooms
            {
                Name = "Phòng thích nghi",
                Description = "Giảm sốc cây con, chuẩn bị ra môi trường tự nhiên",
                Status = 1
            };

            await context.AddRangeAsync(
                tissueCultureLab,
                multiplicationLab,
                rootingLab,
                acclimatizationLab
            );
            await context.SaveChangesAsync();
        }
    }
}
