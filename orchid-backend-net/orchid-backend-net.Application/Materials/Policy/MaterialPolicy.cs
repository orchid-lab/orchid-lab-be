using orchid_backend_net.Domain.Common.Const;

namespace orchid_backend_net.Application.Materials.Policy
{
    public static class MaterialPolicy
    {
        public static readonly HashSet<string> ValidCategories =
        [
                MaterialCategories.CATEGORY_PREPARE_ROOM,
                MaterialCategories.CATEGORY_WASH_AREA,
                MaterialCategories.CATEGORY_STERILIZE_ROOM,
                MaterialCategories.CATEGORY_CULTURE_ROOM,
                MaterialCategories.CATEGORY_ACCLIMATIZATION,
                MaterialCategories.CATEGORY_SURGICAL_TOOL,
                MaterialCategories.CATEGORY_CONTAINER
        ];

        public static bool IsValidCategory(string category)
            => ValidCategories.Contains(category);

        public static readonly HashSet<string> ValidUnit =
        [
            Unit.MATERIAL_UNIT,
        ];
        public static bool IsValidUnit(string unit)
            => ValidUnit.Contains(unit);
    }
}
