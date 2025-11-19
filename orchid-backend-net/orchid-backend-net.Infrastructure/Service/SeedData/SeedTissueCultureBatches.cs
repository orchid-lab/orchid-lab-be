using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedTissueCultureBatches
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<TissueCultureBatches>().AnyAsync())
            {
                var labRooms = await context.Set<LabRooms>().ToListAsync();
                var tissueBatches = new List<TissueCultureBatches>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Batch A1",
                        Description = "First orchid batch",
                        LabRoomID = labRooms.FirstOrDefault()?.ID,
                        Status = true
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Batch B1",
                        Description = "Second orchid batch",
                        LabRoomID = labRooms.Skip(1).FirstOrDefault()?.ID,
                        Status = true
                    }
                };

                await context.Set<TissueCultureBatches>().AddRangeAsync(tissueBatches);
                await context.SaveChangesAsync();
            }
        }
    }
}
