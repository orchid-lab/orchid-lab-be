using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedMethodStageDefinition
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<MethodStageDefinition>().AnyAsync())
            {
                var methods = new List<MethodStageDefinition>()
                {
                    new() { Name = "Lấy mẫu (Sampling)", Description = "Lựa chọn các bộ phận như đỉnh sinh trưởng, mắt ngủ hoặc lát cắt mô phù hợp."},
                    new() { Name = "Khử trùng mẫu (Sterilization)", Description = "Làm sạch và diệt khuẩn mẫu vật bằng hóa chất (cồn 75%, HgCl2) trước khi cấy." },
                    new() { Name = "Nuôi cấy khởi động (Initiation)", Description = "Kích thích mẫu vật hình thành thể tiền chồi (Protocorm) hoặc chồi trực tiếp trong môi trường dinh dưỡng."},
                    new() { Name = "Nhân nhanh (Multiplication)", Description = "Cấy chuyển và chia cắt mẫu nhiều lần để tăng số lượng cây con theo cấp số nhân."},
                    new() { Name = "Tạo cây hoàn chỉnh (Rooting)", Description = "Chuyển mẫu sang môi trường kích thích ra rễ và phát triển lá hoàn chỉnh."},
                    new() { Name =  "Huấn luyện và ra vườn (Acclimatization)", Description = "Đưa cây con từ ống nghiệm ra môi trường tự nhiên để thích nghi."}

                };

                await context.Set<MethodStageDefinition>().AddRangeAsync(methods);
                await context.SaveChangesAsync();
            }
        }
    }
}
