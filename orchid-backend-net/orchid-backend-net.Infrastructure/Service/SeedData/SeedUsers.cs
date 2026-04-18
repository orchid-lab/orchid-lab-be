using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedUsers
    {
        private const string AdminAvatarUrl = "https://res.cloudinary.com/dfrkphimv/image/upload/v1776084528/user-avatar/Untitled-1_zchmho.jpg";
        private const string ResearcherAvatarUrl = "https://res.cloudinary.com/dfrkphimv/image/upload/v1776084904/user-avatar/Untitled-2_z1prea.jpg";
        private const string TechnicianAvatarUrl = "https://res.cloudinary.com/dfrkphimv/image/upload/v1776085118/user-avatar/Untitled-3_typw79.jpg";

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
                        AvatarUrl = AdminAvatarUrl,
                        RoleID = 1,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Lab Technician",
                        Email = "tech@email.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("tech1234"),
                        PhoneNumber = "213133311221",
                        AvatarUrl = TechnicianAvatarUrl,
                        RoleID = 3,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Researcher",
                        Email = "researcher@email.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("research1234"),
                        PhoneNumber = "31312213132",
                        AvatarUrl = ResearcherAvatarUrl,
                        RoleID = 2,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "System"
                    }
                };

                await context.Set<Users>().AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }
    }
}
