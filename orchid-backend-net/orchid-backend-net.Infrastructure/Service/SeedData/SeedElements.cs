using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedElements
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!context.Set<Elements>().Any())
            {
                var elements = new List<Elements>
                {
                    new() { ID = Guid.NewGuid().ToString(), Name = "MS Medium", Description = "Murashige and Skoog medium - môi trường cơ bản trong nuôi cấy mô", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Agar", Description = "Chất đông đặc môi trường nuôi cấy", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Sucrose", Description = "Nguồn carbon trong môi trường nuôi cấy", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "NAA", Description = "Naphthaleneacetic acid - chất điều hòa sinh trưởng (auxin)", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "BA", Description = "Benzyladenine - chất điều hòa sinh trưởng (cytokinin)", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Activated Charcoal", Description = "Than hoạt tính giúp hấp phụ chất ức chế", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Ethanol 70%", Description = "Dùng để khử trùng dụng cụ và mẫu", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Tween 20", Description = "Chất hoạt động bề mặt hỗ trợ khử trùng", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Calcium Hypochlorite", Description = "Chất khử trùng mẫu vật", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Scalpel", Description = "Dao mổ dùng trong cắt mẫu nuôi cấy mô", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Petri Dish", Description = "Đĩa nuôi cấy mô", Status = true },
                    new() { ID = Guid.NewGuid().ToString(), Name = "Pollen Tube Growth Medium", Description = "Môi trường nuôi ống phấn trong lai tạo cây", Status = true }
                };
                foreach (var element in elements)
                {
                    if (!await context.Set<Elements>().AnyAsync(e => e.Name == element.Name))
                    {
                        context.Set<Elements>().Add(element);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
