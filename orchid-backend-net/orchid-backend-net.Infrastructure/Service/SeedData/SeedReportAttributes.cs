using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedReportAttributes
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<ReportAttributes>().AnyAsync())
            {
                var reports = await context.Set<Reports>().ToListAsync();
                var referents = await context.Set<Referents>().ToListAsync();

                var width = referents.FirstOrDefault(r => r.Name == "Chiều cao mầm");
                var roots = referents.FirstOrDefault(r => r.Name == "Số rễ");

                var reportAttributes = new List<ReportAttributes>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ReportID = reports[0].ID,
                        ReferentID = width.ID,
                        Name = "Chiều cao mầm thực tế",
                        Value = 3.5m,
                        Status = 0
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ReportID = reports[1].ID,
                        ReferentID = roots.ID,
                        Name = "Số rễ thực tế",
                        Value = 5,
                        Status = 0
                    }
                };

                await context.Set<ReportAttributes>().AddRangeAsync(reportAttributes);
                await context.SaveChangesAsync();
            }
        }
    }
}
