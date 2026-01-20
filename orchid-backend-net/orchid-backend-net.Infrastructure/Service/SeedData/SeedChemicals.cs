using Microsoft.EntityFrameworkCore;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.Common.Const;

namespace orchid_backend_net.Infrastructure.Service.SeedData
{
    public static class SeedChemicals
    {
        public static async Task SeedAsync(DbContext context)
        {
            if (!await context.Set<Chemicals>().AnyAsync())
            {
                var chemicals = new List<Chemicals>
                {
                    //Khử trùng 
                    new() { Name = "Ethanol 70 - 75%", Category = ChemicalCategories.DISINFECTANT, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "NaOCL", Category = ChemicalCategories.DISINFECTANT, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "HgCl2", Category = ChemicalCategories.DISINFECTANT, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "Tween-20", Category = ChemicalCategories.SOLVENT, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "Nước cất vô trùng", Category = ChemicalCategories.SOLVENT, ConcentrationUnit= Unit.CHEMICAL_UNIT},

                    //Chất điều hòa sinh trưởng
                    new() { Name = "BA (6-BAP)", Category = ChemicalCategories.PLANT_GROWTH_REGULATOR, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "Kinetin", Category = ChemicalCategories.PLANT_GROWTH_REGULATOR, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "NAA", Category = ChemicalCategories.PLANT_GROWTH_REGULATOR, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "IBA", Category = ChemicalCategories.PLANT_GROWTH_REGULATOR, ConcentrationUnit = Unit.CHEMICAL_UNIT},
                    new() { Name = "2,4-D", Category = ChemicalCategories.PLANT_GROWTH_REGULATOR, ConcentrationUnit = Unit.CHEMICAL_UNIT},


                    // Khoáng đa lượng
                    new() { Name = "NH4NO3", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "(NH4)2SO4", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "CaCl2.2H2O", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Ca(NO3)2.4H2O", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Ca3(PO4)2.2H2O", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "MgSO4.7H2O", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "KNO3", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "K2SO4", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "KH2PO4", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "NaH2PO4", Category = ChemicalCategories.MACRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },

                    // Khoáng vi lượng
                    new() { Name = "H3BO3", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit =  Unit.CHEMICAL_UNIT },
                    new() { Name = "CoCl2.6H2O", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "CuSO4.5H2O", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Na2EDTA", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "FeSO4.7H2O", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "MnSO4.H2O", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "KI", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Na2MoO4.2H2O", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "ZnSO4.7H2O", Category = ChemicalCategories.MICRO_MINERALS, ConcentrationUnit = Unit.CHEMICAL_UNIT },

                    // Chất hữu cơ
                    new() { Name = "Myo-inositol", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Biotine", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Cancipentothenote", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Glycine", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Acid nicotinic", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Pyridoxine HCl", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT },
                    new() { Name = "Thiamine HCl", Category = ChemicalCategories.ORGANIC_SUBTANCES, ConcentrationUnit = Unit.CHEMICAL_UNIT }
                };

                await context.Set<Chemicals>().AddRangeAsync(chemicals);
                await context.SaveChangesAsync();
            }
        }
    }
}
