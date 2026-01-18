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
                BatchSize = 120,
                IsBatching = false
            },
            new()
            {
                LabRoomId = labRooms["Phòng cấy mô"].ID,
                LabRoom = labRooms["Phòng cấy mô"],
                BatchName = "TC-2026-02",
                BatchSize = 150,
                IsBatching = true
            },

            // ===== Phòng nhân chồi =====
            new()
            {
                LabRoomId = labRooms["Phòng nhân chồi"].ID,
                LabRoom = labRooms["Phòng nhân chồi"],
                BatchName = "MP-2026-01",
                BatchSize = 300,
                IsBatching = true
            },
            new()
            {
                LabRoomId = labRooms["Phòng nhân chồi"].ID,
                LabRoom = labRooms["Phòng nhân chồi"],
                BatchName = "MP-2026-02",
                BatchSize = 280,
                IsBatching = false
            },

            // ===== Phòng ra rễ =====
            new()
            {
                LabRoomId = labRooms["Phòng ra rễ"].ID,
                LabRoom = labRooms["Phòng ra rễ"],
                BatchName = "RT-2026-01",
                BatchSize = 220,
                IsBatching = true
            },

            // ===== Phòng thích nghi =====
            new()
            {
                LabRoomId = labRooms["Phòng thích nghi"].ID,
                LabRoom = labRooms["Phòng thích nghi"],
                BatchName = "AC-2026-01",
                BatchSize = 200,
                IsBatching = false
            }
        };

                await context.Set<Batches>().AddRangeAsync(batches);
                await context.SaveChangesAsync();
            }
        }
    }
}
