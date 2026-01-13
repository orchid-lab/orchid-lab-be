using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedMethod
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Methods>().AnyAsync())
            {
                var method = new List<Methods>()
                {
                    new() { Name = "Nuôi cấy mô tế bào (Invitro)", Description = "Phương pháp nuôi dưỡng đỉnh sinh trưởng, mắt ngủ hoặc các mô tế bào trong môi trường vô trùng để tạo cây con."},
                    new() { Name = "Nuôi cấy lớp lát mỏng (TCL)", Description = "Kỹ thuật cắt mẫu vật thành các lát cực mỏng (0,3 - 0,5mm) để tăng khả năng tái sinh và nhân giống nhanh."},
                    new() { Name = "Tách bụi (Division)", Description = "Phương pháp tách các giả hành hoặc nhánh từ cây mẹ đã phát triển chật chậu (áp dụng cho lan đa thân)."},
                    new() { Name = "Nhân giống bằng thân giả", Description = "Sử dụng các thân già không còn lá, cắt đoạn và kích thích các mắt ngủ nảy mầm."},
                    new() { Name = "Tách nhánh", Description = "Cắt phần ngọn hoặc đoạn thân có rễ (áp dụng cho lan đơn thân như Vanda, Arachnis)."}
                };
                await context.Set<Methods>().AddRangeAsync(method);
                await context.SaveChangesAsync();
            }
        }
    }
}
