using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedUsers
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Users>().AnyAsync())
            {
                var users = new List<Users>
                {
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Admin User",
                        Email = "admin@email.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin1234"),
                        PhoneNumber = "1234567890",
                        RoleID = 1,
                        Status = true
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Lab Technician",
                        Email = "tech@email.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("tech1234"),
                        PhoneNumber = "213133311221",
                        RoleID = 3,
                        Status = true
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Researcher",
                        Email = "researcher@email.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("research1234"),
                        PhoneNumber = "31312213132",
                        RoleID = 2,
                        Status = true
                    }
                };

                await context.Set<Users>().AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }
    }
}
