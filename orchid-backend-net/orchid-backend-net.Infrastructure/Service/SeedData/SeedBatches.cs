using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedBatches
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Batches>().AnyAsync())
            {
                var labRooms = await context.Set<LabRooms>()
            .ToDictionaryAsync(l => l.Name, l => l);

                var batches = new List<Batches>
        {
            // ===== Phòng cấy mô =====
            new()
            {
                LabRoomId = labRooms["Phòng cấy mô"].ID,
                LabRoom = labRooms["Phòng cấy mô"],
                BatchName = "TC-2026-01",
                BatchSizeWidth = 1200,
                BatchSizeHeight = 1500,
                WidthUnit = "mm",
                HeightUnit = "mm",
                Status = Domain.Common.Enum.BatchStatus.Ready,
            },
            new()
            {
                LabRoomId = labRooms["Phòng cấy mô"].ID,
                LabRoom = labRooms["Phòng cấy mô"],
                BatchName = "TC-2026-02",
                BatchSizeWidth = 1500,
                BatchSizeHeight = 1800,
                WidthUnit = "mm",
                HeightUnit = "mm",
                Status = Domain.Common.Enum.BatchStatus.InUse,
            },

            // ===== Phòng nhân chồi =====
            new()
            {
                LabRoomId = labRooms["Phòng nhân chồi"].ID,
                LabRoom = labRooms["Phòng nhân chồi"],
                BatchName = "MP-2026-01",
                BatchSizeWidth = 3000,
                BatchSizeHeight = 2000,
                WidthUnit = "mm",
                HeightUnit = "mm",
                Status = Domain.Common.Enum.BatchStatus.Ready
            },
            new()
            {
                LabRoomId = labRooms["Phòng nhân chồi"].ID,
                LabRoom = labRooms["Phòng nhân chồi"],
                BatchName = "MP-2026-02",
                BatchSizeWidth = 2800,
                BatchSizeHeight = 2200,
                WidthUnit = "mm",
                HeightUnit = "mm",
                Status = Domain.Common.Enum.BatchStatus.Ready   
            },

            // ===== Phòng ra rễ =====
            new()
            {
                LabRoomId = labRooms["Phòng ra rễ"].ID,
                LabRoom = labRooms["Phòng ra rễ"],
                BatchName = "RT-2026-01",
                BatchSizeWidth = 2200,
                BatchSizeHeight = 1900,
                WidthUnit = "mm",
                HeightUnit = "mm",
                Status = Domain.Common.Enum.BatchStatus.Ready,
            },

            // ===== Phòng thích nghi =====
            new()
            {
                LabRoomId = labRooms["Phòng thích nghi"].ID,
                LabRoom = labRooms["Phòng thích nghi"],
                BatchName = "AC-2026-01",
                BatchSizeWidth = 2000,
                BatchSizeHeight = 1800,
                WidthUnit = "mm",
                HeightUnit = "mm",
                Status = Domain.Common.Enum.BatchStatus.Ready,
            }
        };

                await context.Set<Batches>().AddRangeAsync(batches);
                await context.SaveChangesAsync();
            }
        }
    }
}
