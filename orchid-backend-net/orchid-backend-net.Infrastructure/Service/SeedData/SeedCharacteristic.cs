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

                foreach (var characteristic in characteristics)
                {
                    characteristic.Description ??= characteristic.Code switch
                    {
                        "PLANT_HEIGHT" => "Chiều cao tổng thể của cây đo từ gốc đến đỉnh.",
                        "LEAF_LENGTH" => "Chiều dài trung bình của lá trưởng thành.",
                        "LEAF_WIDTH" => "Chiều rộng trung bình của lá trưởng thành.",
                        "LEAF_THICKNESS" => "Độ dày mô lá phản ánh sức sống và khả năng thích nghi.",
                        "LEAF_COUNT" => "Tổng số lá hữu hiệu trên cây tại thời điểm đo.",
                        "FLOWER_DIAMETER" => "Đường kính trung bình của hoa khi nở hoàn toàn.",
                        "FLOWER_COUNT_PER_SPIKE" => "Số lượng hoa trên mỗi phát hoa.",
                        "FLOWER_COLOR_PRIMARY" => "Màu chủ đạo của hoa, mã hóa dưới dạng RGB.",
                        "FLOWER_COLOR_SECONDARY" => "Màu phụ/điểm nhấn của hoa, mã hóa dưới dạng RGB.",
                        "DAYS_TO_FLOWERING" => "Số ngày từ giai đoạn sinh trưởng đến khi ra hoa.",
                        "FLOWER_LIFESPAN" => "Thời gian duy trì hoa nở trong điều kiện tiêu chuẩn.",
                        "SURVIVAL_RATE" => "Tỷ lệ cây hoặc mẫu còn sống tại mốc đánh giá.",
                        _ => "Chỉ số đặc trưng phục vụ theo dõi và so sánh giống."
                    };
                }

                await context.Set<Characteristic>().AddRangeAsync(characteristics);
                await context.SaveChangesAsync();
            }
        }
    }
}
