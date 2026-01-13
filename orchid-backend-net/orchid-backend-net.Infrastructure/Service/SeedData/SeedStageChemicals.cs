using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Infrastructure.Service.SeedData.Const;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedStageChemicals
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<StageChemicals>().AnyAsync())
                return;

            var stages = await context.Set<MethodStages>()
                .Include(s => s.MethodStageDefinition)
                .ToListAsync();

            var chemicals = await context.Set<Chemicals>().ToListAsync();

            int ChemicalId(string name)
                => chemicals.First(c => c.Name == name).ID;

            IEnumerable<StageChemicals> Map(
                string stageKeyword,
                params string[] chemicalNames)
            {
                return stages
                    .Where(s => s.MethodStageDefinition.Name.Contains(stageKeyword))
                    .SelectMany(s => chemicalNames.Select(cn =>
                        new StageChemicals
                        {
                            StageId = s.ID,
                            ChemicalId = ChemicalId(cn)
                        }));
            }

            var data = new List<StageChemicals>();

            // =========================
            // STERILIZATION
            // =========================
            data.AddRange(Map(
                "Khử trùng",
                ChemicalNames.ETHANOL,
                ChemicalNames.NAOCL,
                ChemicalNames.HGCL2,
                ChemicalNames.TWEEN20,
                ChemicalNames.DISTILLED_WATER
            ));

            // =========================
            // INITIATION
            // =========================
            data.AddRange(Map(
                "Nuôi cấy khởi động",
                ChemicalNames.BAP,
                ChemicalNames.KINETIN,
                ChemicalNames.D24
            ));

            // =========================
            // MULTIPLICATION
            // =========================
            data.AddRange(Map(
                "Nhân nhanh",
                ChemicalNames.BAP,
                ChemicalNames.KINETIN
            ));

            // =========================
            // ROOTING
            // =========================
            data.AddRange(Map(
                "Tạo cây hoàn chỉnh",
                ChemicalNames.NAA,
                ChemicalNames.IBA
            ));

            await context.Set<StageChemicals>().AddRangeAsync(data);
            await context.SaveChangesAsync();
        }
    }
}
