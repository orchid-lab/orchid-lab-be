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
                    // =========================
                    // PROTOCORM (MẦM)
                    // =========================
                    new()
                    {
                        Name = "Tỷ lệ nảy mầm",
                        Unit = "%",
                        Description = "Tỷ lệ hạt hình thành protocorm",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Đường kính protocorm",
                        Unit = "mm",
                        Description = "Kích thước trung bình protocorm",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Thời gian nảy mầm",
                        Unit = "ngày",
                        Description = "Số ngày từ gieo đến khi xuất hiện protocorm",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Tỷ lệ sống protocorm",
                        Unit = "%",
                        Description = "Tỷ lệ protocorm sống đến cuối giai đoạn",
                        CharacteristicCode = "SURVIVAL_RATE"
                    },

                    // =========================
                    // SHOOT / PLB (CHỒI)
                    // =========================
                    new()
                    {
                        Name = "Số PLB hình thành",
                        Unit = "cái",
                        Description = "Số PLB được tạo ra trên mỗi mẫu cấy",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Đường kính PLB",
                        Unit = "mm",
                        Description = "Kích thước trung bình PLB",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Thời gian hình thành PLB",
                        Unit = "ngày",
                        Description = "Thời gian từ cấy đến khi xuất hiện PLB",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Số chồi",
                        Unit = "chồi",
                        Description = "Số chồi phát sinh từ PLB",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Chiều cao chồi",
                        Unit = "cm",
                        Description = "Chiều cao trung bình của chồi",
                        CharacteristicCode = "PLANT_HEIGHT"
                    },
                    new()
                    {
                        Name = "Chiều dài thân giả",
                        Unit = "cm",
                        Description = "Chiều dài thân giả của chồi",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Tỷ lệ sống chồi",
                        Unit = "%",
                        Description = "Tỷ lệ chồi sống đến cuối giai đoạn",
                        CharacteristicCode = "SURVIVAL_RATE"
                    },

                    // =========================
                    // PLANTLET (CÂY CON)
                    // =========================
                    new()
                    {
                        Name = "Số lá",
                        Unit = "lá",
                        Description = "Số lá thật trên cây con",
                        CharacteristicCode = "LEAF_COUNT"
                    },
                    new()
                    {
                        Name = "Chiều dài lá",
                        Unit = "cm",
                        Description = "Chiều dài trung bình của lá",
                        CharacteristicCode = "LEAF_LENGTH"
                    },
                    new()
                    {
                        Name = "Chiều rộng lá",
                        Unit = "cm",
                        Description = "Chiều rộng trung bình của lá",
                        CharacteristicCode = "LEAF_WIDTH"
                    },
                    new()
                    {
                        Name = "Độ dày lá",
                        Unit = "mm",
                        Description = "Độ dày trung bình của lá",
                        CharacteristicCode = "LEAF_THICKNESS"
                    },
                    new()
                    {
                        Name = "Số rễ",
                        Unit = "rễ",
                        Description = "Số rễ hữu hiệu của cây con",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Chiều dài rễ",
                        Unit = "cm",
                        Description = "Chiều dài trung bình của rễ",
                        CharacteristicCode = null
                    },
                    new()
                    {
                        Name = "Chiều cao cây con",
                        Unit = "cm",
                        Description = "Chiều cao từ gốc đến đỉnh cây con",
                        CharacteristicCode = "PLANT_HEIGHT"
                    },
                    new()
                    {
                        Name = "Tỷ lệ sống cây con",
                        Unit = "%",
                        Description = "Tỷ lệ cây con sống trước khi ra vườn ươm",
                        CharacteristicCode = "SURVIVAL_RATE"
                    }
                };
                await context.Set<SamplesRequirementsDefinition>().AddRangeAsync(data);
                await context.SaveChangesAsync();
            }   
        }
    }   
}
