namespace orchid_backend_net.Application.Batch.Policy
{
    public static class BatchPolicy
    {
        private readonly static HashSet<string> AllowUnit = [
            "mm",
            "cm"
            ];
        public static bool IsValidUnit(string unit)
        {
            return AllowUnit.Contains(unit);
        }
    }
}
