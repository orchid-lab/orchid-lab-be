using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedDiseases
    {
        public static async Task SeedAsync(DbContext context)
        {
            var set = context.Set<Disease>();

            // Load existing to prevent duplicates by Code
            var existing = await set.AsNoTracking().Select(d => d.Code).ToListAsync();
            var exists = new HashSet<string>(existing, StringComparer.OrdinalIgnoreCase);

            var candidates = new List<Disease>
            {
                new() 
                { 
                    Code = "disease_anthracnose",           
                    Name = "Bệnh thán thư",                
                    Description = "Tổn thương dạng đốm lõm, cháy lan; thường do nấm Colletotrichum spp." 
                },
                new() 
                {
                    Code = "disease_bacterial_wilt",       
                    Name = "Bệnh héo rũ vi khuẩn",      
                    Description = "Héo nhanh, mạch dẫn nâu; thường do vi khuẩn gây tắc mạch." 
                },
                new() 
                { 
                    Code = "disease_blackrot",              
                    Name = "Bệnh thối đen",               
                    Description = "Tổn thương sẫm màu, mô thối đen; tiến triển ẩm ướt/mềm."
                },
                new() 
                { 
                    Code = "disease_brownspots",            
                    Name = "Đốm nâu",                      
                    Description = "Đốm nâu rải rác trên lá/thân; có thể do nấm/vi khuẩn." 
                },
                new() 
                { 
                    Code = "disease_mold_bac",              
                    Name = "Mốc vi khuẩn",                 
                    Description = "Mốc/màng do vi khuẩn; thường kèm mùi, nhớt bề mặt." 
                },
                new() 
                {
                    Code = "disease_mold_fungus",        
                    Name = "Mốc nấm",                    
                    Description = "Tơ/bao tử nấm phủ bề mặt; ưa ẩm, thông khí kém." 
                },
                new()
                {
                    Code = "disease_soft_rot", 
                    Name = "Thối nhũn", 
                    Description = "Mô thối nhũn, mùi hôi; thường do vi khuẩn mềm thối."
                },
                new() 
                { 
                    Code = "disease_stemrot",      
                    Name = "Thối thân",
                    Description = "Thối vùng thân/gốc, lõi thân mục; có thể đổ rạp."
                },
                new() 
                {
                    Code = "disease_withered_yellow_root", 
                    Name = "Vàng rễ héo",  
                    Description = "Rễ vàng, cây héo; dinh dưỡng/ký sinh gây suy yếu." 
                },
                new() 
                { 
                    Code = "healthy", 
                    Name = "Khỏe mạnh", 
                    Description = "Không phát hiện dấu hiệu bệnh đáng kể."
                },
                new()
                {
                    Code = "oxidation",     
                    Name = "Oxy hóa",   
                    Description = "Đổi màu do oxy hóa/bỏng nhẹ; không phải tác nhân sống." 
                },
                new()
                {
                    Code = "virus",
                    Name = "Nhiễm virus",
                    Description = "Khảm/vàng loang/biến dạng; tiến triển chậm, khó trị." 
                },
            };

            var toInsert = candidates.Where(d => !exists.Contains(d.Code)).ToList();
            if (toInsert.Count == 0) return;

            await set.AddRangeAsync(toInsert);
            await context.SaveChangesAsync();
        }
    }
}