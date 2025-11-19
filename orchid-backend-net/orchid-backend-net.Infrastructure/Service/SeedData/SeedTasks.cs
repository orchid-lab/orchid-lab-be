using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedTasks
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Tasks>().AnyAsync())
            {
                var now = DateTime.UtcNow;

                var tasks = new List<Tasks>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Researcher = "Dr. Nguyễn Văn A",
                        Name = "Sterilization Task",
                        Description = "Task for sterilizing samples",
                        Start_date = now.AddDays(-3),
                        End_date = now.AddDays(2),
                        Create_at = now,
                        Url = "a",
                        ReportInformation = "Report for sterilization task",
                        IsDaily = true,
                        Status = 2 // InProcess
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Researcher = "Dr. Trần Thị B",
                        Name = "Subculturing Task",
                        Description = "Task for subculturing",
                        Start_date = now,
                        End_date = now.AddDays(5),
                        Create_at = now,
                        Url = "b",
                        ReportInformation = "Report for subculturing task",
                        IsDaily = false,
                        Status = 0 // Assign
                    }
                };

                await context.Set<Tasks>().AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}
