using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedRoles
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Role>().AnyAsync())
            {
                var roles = new List<Role>
                {
                    new() { ID = 1, Name = "Admin", Description = "Administrator with full access", Status = true },
                    new() { ID = 2, Name = "Researcher", Description = "Research staff with limited access", Status = true },
                    new() { ID = 3, Name = "Technician", Description = "Lab technician with operational access", Status = true }
                };

                await context.Set<Role>().AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
