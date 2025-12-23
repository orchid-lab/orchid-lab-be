using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Tests.TestData
{
    public static class UserTestData
    {
        public static Users CreateValidAdminUser()
        {
            return new Users
            {
                ID = "user-1",
                Email = "test@gmail.com",
                Password = "hashed-password",
                PhoneNumber = "012345678",
                Name = "Test User",
                AvatarUrl = "http://example.com/avatar.jpg",
                RoleID = 1, // Admin
                DateOfBirth = new DateOnly(1990, 1, 1),
                Role = new Roles
                {
                    ID = 1,
                    Name = "Admin"
                },
                CreatedDate = DateTime.UtcNow.AddHours(7).AddDays(-10),
                CreatedBy = "admin-user",
                DeletedDate = null
            };
        }

        public static Users CreateValidResearcherUser()
        {
            return new Users
            {
                ID = "user-2",
                Email = "test@gmail.com",
                Password = "hashed-password",
                PhoneNumber = "012345678",
                Name = "Test User",
                DateOfBirth = new DateOnly(1990, 1, 1),
                RoleID = 2, // Researcher
                Role = new Roles
                {
                    ID = 2,
                    Name = "Researcher"
                },
                CreatedDate = DateTime.UtcNow.AddHours(7).AddDays(-10),
                CreatedBy = "admin-user",
                UpdatedDate = DateTime.UtcNow.AddHours(7).AddDays(-5),
                UpdatedBy = "researcher-user",
                DeletedDate = null
            };
        }

        public static Users CreateValidTechnicianUser()
        {
            return new Users
            {
                ID = "user-3",
                Email = "test@gmail.com",
                Password = "hashed-password",
                PhoneNumber = "012345678",
                Name = "Test User",
                DateOfBirth = new DateOnly(1990, 1, 1),
                RoleID = 3, // Technician
                Role = new Roles
                {
                    ID = 3,
                    Name = "Technician"
                },
                CreatedDate = DateTime.UtcNow.AddHours(7).AddDays(-10),
                CreatedBy = "admin-user",
                UpdatedDate = DateTime.UtcNow.AddHours(7).AddDays(-5),
                UpdatedBy = "technician-user",
                DeletedDate = null
            };
        }

        public static Users CreateInvalidRoleUser()
        {
            return new Users
            {
                ID = "user-4",
                Email = "test@gmail.com",
                Password = "hashed-password",
                PhoneNumber = "012345678",
                Name = "Test User",
                RoleID = 4, // Invalid
                DeletedDate = null
            };
        }

        public static Users CreateDeletedTechnicianUser()
        {
            return new Users
            {
                ID = "user-4",
                Email = "test@gmail.com",
                Password = "hashed-password",
                PhoneNumber = "012345678",
                Name = "Test User",
                DateOfBirth = new DateOnly(1990, 1, 1),
                RoleID = 3, // Technician
                Role = new Roles
                {
                    ID = 3,
                    Name = "Technician"
                },
                CreatedDate = DateTime.UtcNow.AddHours(7).AddDays(-10),
                CreatedBy = "admin-user",
                DeletedDate = DateTime.UtcNow.AddHours(7),
                DeletedBy = "admin-user"
            };
        }
    }
}
