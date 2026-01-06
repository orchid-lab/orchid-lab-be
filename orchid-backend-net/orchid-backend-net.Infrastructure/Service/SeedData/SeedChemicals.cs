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
                    new() { ID = 1, Name = "NH4NO3", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 2, Name = "(NH4)2SO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 3, Name = "CaCl2.2H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 4, Name = "Ca(NO3)2.4H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 5, Name = "Ca3(PO4)2.2H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 6, Name = "MgSO4.7H2O", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 7, Name = "KNO3", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 8, Name = "K2SO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 9, Name = "KH2PO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 10, Name = "NaH2PO4", Category = MACRO_MINERALS, ConcentrationUnit = UNIT },

                    // Khoáng vi lượng
                    new() { ID = 11, Name = "H3BO3", Category = MICRO_MINERALS, ConcentrationUnit =  UNIT },
                    new() { ID = 12, Name = "CoCl2.6H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 13, Name = "CuSO4.5H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 14, Name = "Na2EDTA", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 15, Name = "FeSO4.7H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 16, Name = "MnSO4.H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 17, Name = "KI", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 19, Name = "Na2MoO4.2H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },
                    new() { ID = 20, Name = "ZnSO4.7H2O", Category = MICRO_MINERALS, ConcentrationUnit = UNIT },

                    // Chất hữu cơ
                    new() { ID = 21, Name = "Myo-inositol", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { ID = 22, Name = "Biotine", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { ID = 23, Name = "Cancipentothenote", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { ID = 24, Name = "Glycine", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { ID = 25, Name = "Acid nicotinic", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { ID = 26,Name = "Pyridoxine HCl", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT },
                    new() { ID = 27, Name = "Thiamine HCl", Category = ORGANIC_SUBTANCES, ConcentrationUnit = UNIT }
                };

                await context.Set<Chemicals>().AddRangeAsync(chemicals);
                await context.SaveChangesAsync();
            }
        }
    }
}
