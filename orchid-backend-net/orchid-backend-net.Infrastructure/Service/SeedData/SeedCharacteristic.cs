using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedCharacteristic
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Characteristic>().AnyAsync())
            {
                var characteristics = new List<Characteristic>
                {
                    new() { Code = "PLANT_HEIGHT", Name = "Chiều cao cây", Unit = "cm", Description = "Chiều cao cây con hoặc cây trưởng thành, đo từ gốc đến đỉnh sinh trưởng" },
                    new() { Code = "LEAF_COUNT", Name = "Số lá", Unit = "lá", Description = "Số lượng lá thật trên cây" },
                    new() { Code = "ROOT_COUNT", Name = "Số rễ", Unit = "rễ", Description = "Số lượng rễ khỏe, có khả năng sinh trưởng" },
                    new() { Code = "ROOT_LENGTH", Name = "Chiều dài rễ", Unit = "cm", Description = "Chiều dài trung bình của rễ chính" },
                    new() { Code = "FLOWER_COLOR", Name = "Màu hoa", Unit = "nhóm màu", Description = "Màu sắc chủ đạo của hoa (trắng, hồng, vàng, tím…)" },
                    new() { Code = "FLOWER_SIZE", Name = "Kích thước hoa", Unit = "cm", Description = "Đường kính trung bình của hoa" }
                };

                await context.Set<Characteristic>().AddRangeAsync(characteristics);
                await context.SaveChangesAsync();
            }
        }
    }
}
