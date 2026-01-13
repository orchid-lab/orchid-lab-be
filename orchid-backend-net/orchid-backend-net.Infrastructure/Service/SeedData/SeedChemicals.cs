using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedChemicals
    {
        public const string MACRO_MINERALS = "Khoáng đa lượng";
        public const string MICRO_MINERALS = "Khoáng vi lượng";
        public const string ORGANIC_SUBTANCES = "Chất hữu cơ";
        public const string UNIT = "mg/L";
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Chemicals>().AnyAsync())
            {
                var chemicals = new List<Chemicals>
                {
                    // Khoáng đa lượng
                    new() { Name = "NH4NO3", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "(NH4)2SO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "CaCl2.2H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "Ca(NO3)2.4H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "Ca3(PO4)2.2H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "MgSO4.7H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "KNO3", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "K2SO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "KH2PO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "NaH2PO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },

                    // Khoáng vi lượng
                    new() { Name = "H3BO3", Category = MICRO_MINERALS, ConcentrationUnit =  UNIT },
                    new() { Name = "CoCl2.6H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "CuSO4.5H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "Na2EDTA", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "FeSO4.7H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "MnSO4.H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "KI", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "Na2MoO4.2H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { Name = "ZnSO4.7H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },

                    // Chất hữu cơ
                    new() { Name = "Myo-inositol", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { Name = "Biotine", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { Name = "Cancipentothenote", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { Name = "Glycine", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { Name = "Acid nicotinic", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { Name = "Pyridoxine HCl", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { Name = "Thiamine HCl", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT }
                };

                await context.Set<Chemicals>().AddRangeAsync(chemicals);
                await context.SaveChangesAsync();
            }
        }
    }
}
