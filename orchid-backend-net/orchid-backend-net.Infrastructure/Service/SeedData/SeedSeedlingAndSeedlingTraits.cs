using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedSeedlingAndSeedlingTraits
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<Seedlings>().AnyAsync())
                return;

            var characteristics = await context.Set<Characteristic>()
                .ToDictionaryAsync(c => c.Code, c => c);

            // ===== Parent A =====
            var parentA = new Seedlings
            {
                LocalName = "Lan Hồ Điệp Trắng",
                ScientificName = "Phalaenopsis amabilis",
                Description = "Cây mẹ – hoa trắng, sinh trưởng mạnh",
                CreatedBy = "system",
                CreatedDate = DateTime.UtcNow,
            };

            parentA.SeedlingsTraits.AddRange(new[]
            {
        Trait(characteristics["PLANT_HEIGHT"], 55),
        Trait(characteristics["LEAF_LENGTH"], 28),
        Trait(characteristics["LEAF_WIDTH"], 12),
        Trait(characteristics["LEAF_THICKNESS"], 3.2m),
        Trait(characteristics["LEAF_COUNT"], 6),

        Trait(characteristics["FLOWER_DIAMETER"], 9.5m),
        Trait(characteristics["FLOWER_COUNT_PER_SPIKE"], 10),

        // Màu hoa chính: trắng (245, 245, 245)
        Trait(characteristics["FLOWER_COLOR_PRIMARY"], RgbToDecimal(245, 245, 245)),
        // Màu hoa phụ: trắng kem (250, 248, 240)
        Trait(characteristics["FLOWER_COLOR_SECONDARY"], RgbToDecimal(250, 248, 240)),

        Trait(characteristics["DAYS_TO_FLOWERING"], 270),
        Trait(characteristics["FLOWER_LIFESPAN"], 85),
        Trait(characteristics["SURVIVAL_RATE"], 92),
    });

            // ===== Parent B =====
            var parentB = new Seedlings
            {
                LocalName = "Lan Hồ Điệp Hồng",
                ScientificName = "Phalaenopsis aphrodite",
                Description = "Cây bố – hoa hồng nhạt, cánh tròn",
                CreatedBy = "system",
                CreatedDate = DateTime.UtcNow,
            };

            parentB.SeedlingsTraits.AddRange(new[]
            {
        Trait(characteristics["PLANT_HEIGHT"], 50),
        Trait(characteristics["LEAF_LENGTH"], 26),
        Trait(characteristics["LEAF_WIDTH"], 11),
        Trait(characteristics["LEAF_THICKNESS"], 3.0m),
        Trait(characteristics["LEAF_COUNT"], 5),

        Trait(characteristics["FLOWER_DIAMETER"], 10),
        Trait(characteristics["FLOWER_COUNT_PER_SPIKE"], 9),

        // Màu hoa chính: hồng nhạt (255, 180, 200)
        Trait(characteristics["FLOWER_COLOR_PRIMARY"], RgbToDecimal(255, 180, 200)),
        // Màu hoa phụ: hồng đậm (255, 150, 180)
        Trait(characteristics["FLOWER_COLOR_SECONDARY"], RgbToDecimal(255, 150, 180)),

        Trait(characteristics["DAYS_TO_FLOWERING"], 260),
        Trait(characteristics["FLOWER_LIFESPAN"], 80),
        Trait(characteristics["SURVIVAL_RATE"], 90),
    });

            await context.AddRangeAsync(parentA, parentB);
            await context.SaveChangesAsync();

            // ===== Hybrid =====
            var hybrid = new Seedlings
            {
                LocalName = "Lan Hồ Điệp Lai A",
                ScientificName = "Phalaenopsis Hybrid A",
                Description = "Con lai giữa Hồ Điệp Trắng × Hồ Điệp Hồng",
                ParentAId = parentA.ID,
                CreatedBy = "system",
                CreatedDate = DateTime.UtcNow,
            };

            hybrid.SeedlingsTraits.AddRange(new[]
            {
        Trait(characteristics["PLANT_HEIGHT"], 52),
        Trait(characteristics["LEAF_LENGTH"], 27),
        Trait(characteristics["LEAF_WIDTH"], 11.5m),
        Trait(characteristics["LEAF_THICKNESS"], 3.1m),
        Trait(characteristics["LEAF_COUNT"], 6),

        Trait(characteristics["FLOWER_DIAMETER"], 10.2m),
        Trait(characteristics["FLOWER_COUNT_PER_SPIKE"], 11),

       // Màu hoa chính: hồng rất nhạt (248, 210, 220)
        Trait(characteristics["FLOWER_COLOR_PRIMARY"], RgbToDecimal(248, 210, 220)),
        // Màu hoa phụ: hồng pastel (250, 200, 215)
        Trait(characteristics["FLOWER_COLOR_SECONDARY"], RgbToDecimal(250, 200, 215)),

        Trait(characteristics["DAYS_TO_FLOWERING"], 255),
        Trait(characteristics["FLOWER_LIFESPAN"], 88),
        Trait(characteristics["SURVIVAL_RATE"], 94),
    });

            await context.AddAsync(hybrid);
            await context.SaveChangesAsync();
        }

        private static SeedlingsTraits Trait(Characteristic characteristic, decimal value)
        {
            return new SeedlingsTraits
            {
                CharacteristicId = characteristic.ID,
                Value = value
            };
        }

        /// <summary>
        /// Chuyển đổi giá trị RGB (0-255) sang định dạng decimal: RRRGGGBBB
        /// </summary>
        /// <param name="r">Red (0-255)</param>
        /// <param name="g">Green (0-255)</param>
        /// <param name="b">Blue (0-255)</param>
        /// <returns>Giá trị decimal tổng hợp</returns>
        private static decimal RgbToDecimal(int r, int g, int b)
        {
            return r * 1_000_000 + g * 1_000 + b;
        }
    }
}
