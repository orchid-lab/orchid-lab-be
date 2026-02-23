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

                    new() { Code = "PLANT_HEIGHT", Name = "Chiều cao cây trưởng thành", Unit = "cm" },
                    new() { Code = "LEAF_LENGTH", Name = "Chiều dài lá", Unit = "cm" },
                    new() { Code = "LEAF_WIDTH", Name = "Chiều rộng lá", Unit = "cm" },
                    new() { Code = "LEAF_THICKNESS", Name = "Độ dày lá", Unit = "mm" },
                    new() { Code = "LEAF_COUNT", Name = "Số lá trên cây", Unit = "lá" },

                    new() { Code = "FLOWER_DIAMETER", Name = "Đường kính hoa", Unit = "cm" },
                    new() { Code = "FLOWER_COUNT_PER_SPIKE", Name = "Số hoa trên phát hoa", Unit = "hoa" },

                    new() { Code = "FLOWER_COLOR_PRIMARY", Name = "Màu hoa chính", Unit = "RGB" },
                    new() { Code = "FLOWER_COLOR_SECONDARY", Name = "Màu hoa phụ", Unit = "RGB" },

                    new() { Code = "DAYS_TO_FLOWERING", Name = "Thời gian đến ra hoa", Unit = "ngày" },
                    new() { Code = "FLOWER_LIFESPAN", Name = "Thời gian hoa bền", Unit = "ngày" },

                    new() { Code = "SURVIVAL_RATE", Name = "Tỷ lệ sống", Unit = "%" }
                };

                await context.Set<Characteristic>().AddRangeAsync(characteristics);
                await context.SaveChangesAsync();
            }
        }
    }
}
