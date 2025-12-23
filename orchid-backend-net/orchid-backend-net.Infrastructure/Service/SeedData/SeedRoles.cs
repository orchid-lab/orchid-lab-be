using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedRoles
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Roles>().AnyAsync())
            {
                var roles = new List<Roles>()
                {
                    new() {
                        ID = 1,
                        Name = "Admin"
                    },
                    new() {
                        ID = 2,
                        Name = "Researcher"
                    },
                    new() {
                        ID = 3,
                        Name = "Lab Technician"
                    }
                };

                await context.Set<Roles>().AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
