using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.ExperimentLog.Policy
{
    public static class ExperimentLogPolicy
    {
        private static readonly HashSet<ExperimentLogStatus> ValidStatusTransitions = [
            ExperimentLogStatus.Created,
            ExperimentLogStatus.InProgress,
            ExperimentLogStatus.WaitingForChangeStage,
            ExperimentLogStatus.ConfirmChangeStage,
            ExperimentLogStatus.Completed,
            ExperimentLogStatus.Destroyed
            ];

        public static ExperimentLogStatus ValidateStatusChange(string status)
        {
            if (!Enum.TryParse<ExperimentLogStatus>(status, out var newStatus) ||
                !ValidStatusTransitions.Contains(newStatus))
            {
                throw new ArgumentException("Trạng thái thí nghiệm không hợp lệ.");
            }
            return newStatus;
        }
    }
}
