using orchid_backend_net.Domain.Common.Enum;

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

        private readonly static HashSet<BatchStatus> AllowNextStatus = [
            BatchStatus.Ready,
            BatchStatus.Cleaning,
            BatchStatus.Maintenance,
            ];

        public static BatchStatus ValidateBatchStatusChange(string requestedStatus)
        {
            if (!Enum.TryParse<BatchStatus>(requestedStatus, out var status) || !AllowNextStatus.Contains(status))
            {
                throw new ArgumentException("Trạng thái không hợp lệ");
            }
            return status;
        }
    }
}
