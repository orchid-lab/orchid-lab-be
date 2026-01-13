using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedSampleRequirementDefinition
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<SamplesRequirementsDefinition>().AnyAsync())
            {


            var data = new List<SamplesRequirementsDefinition>
            {
                    // === THÂN GIẢ  ===
                new() 
                {
                    ID = Guid.Parse("10000000-0000-0000-0000-000000000001").ToString(),
                    CharacteristicCode = "STEM_COUNT",
                    Name = "Số thân giả",
                    Description = "Số lượng thân giả trên cây",
                    Unit = "cái",
                    MinValue = 1,
                    MaxValue = 3,
                    DefaultExpectedValue = 2
                },
                new() 
                {
                    ID = Guid.Parse("10000000-0000-0000-0000-000000000002").ToString(),
                    CharacteristicCode = "PLANT_HEIGHT",
                    Name = "Chiều cao cây",
                    Description = "Chiều cao từ gốc đến ngọn",
                    Unit = "cm",
                    MinValue = 5,
                    MaxValue = 15,
                    DefaultExpectedValue = 10
                },
                new() 
                {
                    ID = Guid.Parse("10000000-0000-0000-0000-000000000003").ToString(),
                    CharacteristicCode = "STEM_CONDITION",
                    Name = "Tình trạng thân/lá",
                    Description = "Đánh giá không nhăn nheo (0=nhăn nhiều, 1=hơi nhăn, 2=không nhăn)",
                    Unit = "mức",
                    MinValue = 0,
                    MaxValue = 2,
                    DefaultExpectedValue = 2
                },

                // === LÁ ===
                new() 
                {
                    ID = Guid.Parse("20000000-0000-0000-0000-000000000001").ToString(),
                    CharacteristicCode = "LEAF_COUNT",
                    Name = "Số lá",
                    Description = "Số lượng lá trên cây",
                    Unit = "lá",
                    MinValue = 3,
                    MaxValue = 12,
                    DefaultExpectedValue = 6
                },
                new() 
                {
                    ID = Guid.Parse("20000000-0000-0000-0000-000000000002").ToString(),
                    CharacteristicCode = "LEAF_COLOR",
                    Name = "Màu sắc lá",
                    Description = "Đánh giá màu xanh (0=vàng, 1=xanh nhạt, 2=xanh vừa, 3=xanh đậm)",
                    Unit = "mức",
                    MinValue = 0,
                    MaxValue = 3,
                    DefaultExpectedValue = 2 // "xanh vừa"
                },

                // === RỄ ===
                new() 
                {
                    ID = Guid.Parse("30000000-0000-0000-0000-000000000001").ToString(),
                    CharacteristicCode = "ROOT_COUNT",
                    Name = "Số rễ",
                    Description = "Số lượng rễ khỏe mạnh",
                    Unit = "rễ",
                    MinValue = 2,
                    MaxValue = 10,
                    DefaultExpectedValue = 3
                },
                new() 
                {
                    ID = Guid.Parse("30000000-0000-0000-0000-000000000002").ToString(),
                    CharacteristicCode = "ROOT_LENGTH",
                    Name = "Chiều dài rễ",
                    Description = "Chiều dài rễ trung bình",
                    Unit = "cm",
                    MinValue = 2,
                    MaxValue = 10,
                    DefaultExpectedValue = 5
                },
            };

            await context.Set<SamplesRequirementsDefinition>().AddRangeAsync(data);
            await context.SaveChangesAsync();
            }
        }

    }
}
