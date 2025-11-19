using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedLabRooms
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<LabRooms>().AnyAsync())
            {
                var labRooms = new List<LabRooms>
                {
                    new() { ID = Guid.NewGuid().ToString(), Name = "Room A", Description = "Sterile lab room A", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Room B", Description = "Growth room B", Status = true },
                };

                await context.Set<LabRooms>().AddRangeAsync(labRooms);
                await context.SaveChangesAsync();
            }
        }
    }
}
