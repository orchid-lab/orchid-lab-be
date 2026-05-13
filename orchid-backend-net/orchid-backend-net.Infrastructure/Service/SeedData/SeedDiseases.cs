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
                new() { Code = "disease_anthracnose", Name = "Bệnh thán thư", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "Anthracnose" },
                new() { Code = "disease_bacterial_wilt", Name = "Bệnh héo rũ vi khuẩn", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "BacterialWilt" },
                new() { Code = "disease_blackrot", Name = "Bệnh thối đen", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "Blackrot" },
                new() { Code = "disease_brownspots", Name = "Đốm nâu", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "Brownspots" },
                new() { Code = "disease_mold_bac", Name = "Mốc vi khuẩn", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "MoldBacterial" },
                new() { Code = "disease_mold_fungus", Name = "Mốc nấm", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "MoldFungus" },
                new() { Code = "disease_soft_rot", Name = "Thối nhũn", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "SoftRot" },
                new() { Code = "disease_stemrot", Name = "Thối thân", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "StemRot" },
                new() { Code = "disease_withered_yellow_root", Name = "Vàng rễ héo", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "WitheredYellowRoot" },
                new() { Code = "healthy", Name = "Khỏe mạnh", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "Healthy" },
                new() { Code = "oxidation", Name = "Oxy hóa", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "Oxidation" },
                new() { Code = "virus", Name = "Nhiễm virus", Description = "...", IsActive = true, CreatedAt = DateTime.UtcNow, OnnxClassName = "Virus" },
            };

            var toInsert = candidates.Where(d => !exists.Contains(d.Code)).ToList();
            if (toInsert.Count == 0) return;

            await set.AddRangeAsync(toInsert);
            await context.SaveChangesAsync();
        }
    }
}