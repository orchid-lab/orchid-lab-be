using orchid_backend_net.Domain.Common.Const;

namespace orchid_backend_net.Application.Chemicals.Policy
{
    public static class ChemicalPolicy
    {
        public static HashSet<string> ChemicalCategoriesValidation =
        [
            ChemicalCategories.MACRO_MINERALS,
            ChemicalCategories.MICRO_MINERALS,
            ChemicalCategories.ORGANIC_SUBTANCES,
            ChemicalCategories.DISINFECTANT,
            ChemicalCategories.SOLVENT,
            ChemicalCategories.PLANT_GROWTH_REGULATOR
        ];

        public static bool IsValidCategory(string category)
        {
            return ChemicalCategoriesValidation.Contains(category);
        }

        public static HashSet<string> ChemicalUnitValidation =
        [
                Unit.CHEMICAL_UNIT,
        ];


        public static bool IsValidUnit(string unit)
        {
            return ChemicalUnitValidation.Contains(unit);
        }
    }
}
