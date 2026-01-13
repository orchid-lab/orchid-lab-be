using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Service.SeedData.Const;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedStageMaterials
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<StageMaterials>().AnyAsync()) return;

            var stages = await context.Set<MethodStages>()
                .Include(x => x.MethodStageDefinition)
                .ToListAsync();

            var materials = await context.Set<Materials>().ToListAsync();

            int M(string name) => materials.First(x => x.Name == name).ID;

            IEnumerable<StageMaterials> Map(string stage, params string[] mats)
                => stages
                    .Where(s => s.MethodStageDefinition.Name.StartsWith(stage))
                    .SelectMany(s => mats.Select(m =>
                        new StageMaterials
                        {
                            StageId = s.ID,
                            MaterialId = M(m)
                        }));

            var data = new List<StageMaterials>();

            // Sampling
            data.AddRange(Map(StageNames.SAMPLING,
                MaterialNames.FORCEPS,
                MaterialNames.SCALPEL,
                MaterialNames.BLADE,
                MaterialNames.TRAY,
                MaterialNames.GLOVES
            ));

            // Sterilization
            data.AddRange(Map(StageNames.STERILIZATION,
                "Vòi nước",
                "Bồn nước",
                "Xà phòng",
                "Cọ rửa chai",
                MaterialNames.AUTOCLAVE,
                "Tủ sấy",
                MaterialNames.FILTER_PAPER,
                MaterialNames.GLOVES,
                MaterialNames.TRAY,
                "Bàn để dụng cụ đã khử trùng"
            ));

            // Initiation
            data.AddRange(Map(StageNames.INITIATION,
                MaterialNames.LAMINAR,
                MaterialNames.CULTURE_BOTTLE,
                "Becher",
                "Cốc đong",
                "Ống đong",
                "Pipette",
                "Đũa thủy tinh",
                "Máy đo pH",
                "Máy khuấy từ",
                "Cân điện tử (2 số)"
            ));

            // Multiplication
            data.AddRange(Map(StageNames.MULTIPLICATION,
                MaterialNames.LAMINAR,
                MaterialNames.CULTURE_BOTTLE,
                "Becher",
                "Cốc đong",
                "Ống đong",
                "Pipette",
                "Đũa thủy tinh",
                "Máy đo pH",
                "Máy khuấy từ",
                "Cân điện tử (2 số)"
            ));

            // Rooting
            data.AddRange(Map(StageNames.ROOTING,
                MaterialNames.LAMINAR,
                MaterialNames.CULTURE_BOTTLE,
                "Becher",
                "Cốc đong",
                "Ống đong",
                "Pipette",
                "Đũa thủy tinh"
            ));

            // Acclimatization
            data.AddRange(Map(StageNames.ACCLIMATIZATION,
                MaterialNames.SEED_TRAY,
                MaterialNames.SUBSTRATE,
                MaterialNames.SPRAYER,
                MaterialNames.RACK,
                "Nhà lưới / mái che"
            ));

            await context.Set<StageMaterials>().AddRangeAsync(data);
            await context.SaveChangesAsync();
        }
    }

}
