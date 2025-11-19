using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedTaskAssigns
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<TasksAssign>().AnyAsync())
            {
                // Lấy user Lab Technician
                var technician = await context.Set<Users>()
                    .FirstOrDefaultAsync(u => u.RoleID == 3); // RoleID 3 = Lab Technician

                if (technician == null)
                    throw new ArgumentException("Lab Technician user not found!");

                // Lấy task đầu tiên để gán
                var task = await context.Set<Tasks>().FirstOrDefaultAsync();
                if (task == null)
                    throw new ArgumentException("No tasks found to assign!");

                var taskAssign = new TasksAssign
                {
                    ID = Guid.NewGuid().ToString(),
                    TechnicianID = technician.ID,
                    TaskID = task.ID,
                    Status = false // Chưa hoàn thành
                };

                await context.Set<TasksAssign>().AddAsync(taskAssign);
                await context.SaveChangesAsync();
            }
        }
    }
}
