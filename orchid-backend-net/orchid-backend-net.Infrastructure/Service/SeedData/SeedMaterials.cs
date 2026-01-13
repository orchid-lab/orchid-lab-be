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
                    new() { Name = "Máy cất nước", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Máy đo pH", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Máy khuấy từ", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Tủ lạnh đựng hóa chất", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Cân điện tử (Cân 2 số)", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Muỗng", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Vá", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Đũa thủy tinh", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Cốc đong", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Ống đong", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Pipette", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Đĩa Petri", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Chai thủy tinh", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Becher", Category = CATEGORY_PREPARE_ROOM, Unit = UNIT_PIECE },

                    // Khu vực rửa dụng cụ
                    new() { Name = "Vòi nước", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { Name = "Bồn nước", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { Name = "Giá, kệ để chai", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { Name = "Xà phòng", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },
                    new() { Name = "Cọ rửa chai", Category = CATEGORY_WASH_AREA, Unit = UNIT_PIECE },

                    // Phòng khử trùng
                    new() { Name = "Nồi hấp (autoclave)", Category = CATEGORY_STERILIZE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Tủ sấy", Category = CATEGORY_STERILIZE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Bàn để môi trường và dụng cụ đã khử trùng", Category = CATEGORY_STERILIZE_ROOM, Unit = UNIT_PIECE },

                    // Phòng cấy
                    new() { Name = "Phòng cấy", Category = CATEGORY_CULTURE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Tủ cấy - Loại tủ kín", Category = CATEGORY_CULTURE_ROOM, Unit = UNIT_PIECE },
                    new() { Name = "Tủ cấy - Loại laminar flow", Category = CATEGORY_CULTURE_ROOM, Unit = UNIT_PIECE }
                };

                await context.Set<Materials>().AddRangeAsync(materials);
                await context.SaveChangesAsync();
            }
        }
    }
}
