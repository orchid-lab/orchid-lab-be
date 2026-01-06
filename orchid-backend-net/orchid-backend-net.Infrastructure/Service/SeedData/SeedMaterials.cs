using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedMaterials
    {
        // Constants cho Category
        private const string CATEGORY_PREPARE_ROOM = "Phòng chuẩn bị môi trường nuôi cấy";
        private const string CATEGORY_WASH_AREA = "Khu vực rửa dụng cụ";
        private const string CATEGORY_STERILIZE_ROOM = "Phòng khử trùng";
        private const string CATEGORY_CULTURE_ROOM = "Phòng cấy";

        // Constants cho Unit
        private const string UNIT_PIECE = "cái";

        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Materials>().AnyAsync())
            {
                var materials = new List<Materials>
                {
                    // Phòng chuẩn bị môi trường nuôi cấy
                    new() { ID = 1, Name = "Máy cất nước", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 2, Name = "Máy đo pH", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 3, Name = "Máy khuấy từ", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 4, Name = "Tủ lạnh đựng hóa chất", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 5, Name = "Cân điện tử (Cân 2 số)", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 6, Name = "Muỗng", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 7, Name = "Vá", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 8, Name = "Đũa thủy tinh", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 9, Name = "Cốc đong", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 10, Name = "Ống đong", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 11, Name = "Pipette", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 12, Name = "Đĩa Petri", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 13, Name = "Chai thủy tinh", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 14, Name = "Becher", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },

                    // Khu vực rửa dụng cụ
                    new() { ID = 15, Name = "Vòi nước", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { ID = 16, Name = "Bồn nước", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { ID = 17, Name = "Giá, kệ để chai", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { ID = 18, Name = "Xà phòng", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { ID = 19, Name = "Cọ rửa chai", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },

                    // Phòng khử trùng
                    new() { ID = 20, Name = "Nồi hấp (autoclave)", Category = CATEGORY_STERILIZE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 21, Name = "Tủ sấy", Category = CATEGORY_STERILIZE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 22, Name = "Bàn để môi trường và dụng cụ đã khử trùng", Category = CATEGORY_STERILIZE_ROOM, Unit = UNIT_PIECE },

                    // Phòng cấy
                    new() { ID = 23, Name = "Phòng cấy", Category = CATEGORY_CULTURE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 24, Name = "Tủ cấy - Loại tủ kín", Category = CATEGORY_CULTURE_ROOM, Unit = UNIT_PIECE },
                    new() { ID = 25, Name = "Tủ cấy - Loại laminar flow", Category = CATEGORY_CULTURE_ROOM, Unit = UNIT_PIECE }
                };

                await context.Set<Materials>().AddRangeAsync(materials);
                await context.SaveChangesAsync();
            }
        }
    }
}
