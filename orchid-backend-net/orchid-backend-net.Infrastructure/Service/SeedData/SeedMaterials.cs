using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Common.Const;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedMaterials
    {
        // Constants cho Unit

        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Materials>().AnyAsync())
            {
                var materials = new List<Materials>
        {
            // ========================
            // PHÒNG CHUẨN BỊ MÔI TRƯỜNG
            // ========================
            new() { Name = "Máy cất nước", Category = MaterialCategories.CATEGORY_PREPARE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Máy đo pH", Category = MaterialCategories.CATEGORY_PREPARE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Máy khuấy từ", Category = MaterialCategories.CATEGORY_PREPARE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Tủ lạnh đựng hóa chất", Category = MaterialCategories.CATEGORY_PREPARE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Cân điện tử (2 số)", Category = MaterialCategories.CATEGORY_PREPARE_ROOM, Unit = Unit.MATERIAL_UNIT },

            // ========================
            // DỤNG CỤ CHỨA
            // ========================
            new() { Name = "Becher", Category = MaterialCategories.CATEGORY_CONTAINER, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Cốc đong", Category = MaterialCategories.CATEGORY_CONTAINER, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Ống đong", Category = MaterialCategories.CATEGORY_CONTAINER, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Chai thủy tinh nuôi cấy", Category = MaterialCategories.CATEGORY_CONTAINER, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Đĩa Petri", Category = MaterialCategories.CATEGORY_CONTAINER, Unit = Unit.MATERIAL_UNIT },

            // ========================
            // KHU RỬA DỤNG CỤ
            // ========================
            new() { Name = "Vòi nước", Category = MaterialCategories.CATEGORY_WASH_AREA, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Bồn nước", Category = MaterialCategories.CATEGORY_WASH_AREA, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Giá, kệ để chai", Category = MaterialCategories.CATEGORY_WASH_AREA, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Xà phòng", Category = MaterialCategories.CATEGORY_WASH_AREA, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Cọ rửa chai", Category = MaterialCategories.CATEGORY_WASH_AREA, Unit = Unit.MATERIAL_UNIT },

            // ========================
            // DỤNG CỤ THAO TÁC VÔ TRÙNG
            // ========================
            new() { Name = "Nhíp", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Dao mổ", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Lưỡi lam", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Khay inox", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Giấy lọc", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Găng tay y tế", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Pipette", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Đũa thủy tinh", Category = MaterialCategories.CATEGORY_SURGICAL_TOOL, Unit = Unit.MATERIAL_UNIT },

            // ========================
            // PHÒNG KHỬ TRÙNG
            // ========================
            new() { Name = "Nồi hấp (Autoclave)", Category = MaterialCategories.CATEGORY_STERILIZE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Tủ sấy", Category = MaterialCategories.CATEGORY_STERILIZE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Bàn để dụng cụ đã khử trùng", Category = MaterialCategories.CATEGORY_STERILIZE_ROOM, Unit = Unit.MATERIAL_UNIT },

            // ========================
            // PHÒNG CẤY
            // ========================
            new() { Name = "Tủ cấy – Laminar Flow", Category = MaterialCategories.CATEGORY_CULTURE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Tủ cấy – Loại kín", Category = MaterialCategories.CATEGORY_CULTURE_ROOM, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Phòng cấy", Category = MaterialCategories.CATEGORY_CULTURE_ROOM, Unit = Unit.MATERIAL_UNIT },

            // ========================
            // HUẤN LUYỆN & RA VƯỜN
            // ========================
            new() { Name = "Khay ươm cây", Category = MaterialCategories.CATEGORY_ACCLIMATIZATION, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Giá thể trồng (xơ dừa / than / rêu)", Category = MaterialCategories.CATEGORY_ACCLIMATIZATION, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Bình phun sương", Category = MaterialCategories.CATEGORY_ACCLIMATIZATION, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Kệ trồng cây", Category = MaterialCategories.CATEGORY_ACCLIMATIZATION, Unit = Unit.MATERIAL_UNIT },
            new() { Name = "Nhà lưới / mái che", Category = MaterialCategories.CATEGORY_ACCLIMATIZATION, Unit = Unit.MATERIAL_UNIT }
        };

                await context.Set<Materials>().AddRangeAsync(materials);
                await context.SaveChangesAsync();
            }
        }
    }
}
