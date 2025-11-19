using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedReports
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Reports>().AnyAsync())
            {
                var samples = await context.Set<Samples>().ToListAsync();
                var users = await context.Set<Users>().ToListAsync();
                var technicians = users.Where(u => u.RoleID == 3);

                var reports = new List<Reports>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        SampleID = samples.First().ID,
                        TechnicianID = technicians.First().ID,
                        Name = "Báo cáo lần 1 - Sample A",
                        Description = "Theo dõi mầm sau cấy",
                        IsLatest = true,
                        Status = 0
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        SampleID = samples.Last().ID,
                        TechnicianID = technicians.First().ID,
                        Name = "Báo cáo lần 1 - Sample B",
                        Description = "Theo dõi rễ phát triển",
                        IsLatest = true,
                        Status = 0
                    }
                };

                await context.Set<Reports>().AddRangeAsync(reports);
                await context.SaveChangesAsync();
            }

        }
    }
}
