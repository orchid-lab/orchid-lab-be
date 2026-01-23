using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.ExperimentLog.Helper
{
    public static class ExperimentLogStatusDispatcher
    {
        public static void Dispatch(
            ExperimentLogs el,
            Domain.Common.Enum.ExperimentLogStatus nextStatus,
            MethodStages nextStage,
            string? reason)
        {
            switch(nextStatus)
            {
                case Domain.Common.Enum.ExperimentLogStatus.InProgress:
                    el.Start();
                    break;
                case Domain.Common.Enum.ExperimentLogStatus.WaitingForChangeStage:
                    el.PendingToChangeStage();
                    break;
                case Domain.Common.Enum.ExperimentLogStatus.ConfirmChangeStage:
                    el.MoveToNextStage(nextStage, nextStage.Order);
                    break;
                case Domain.Common.Enum.ExperimentLogStatus.Destroyed:
                    el.DestroyBecauseAllSamplesInfected(reason);
                    break;
                case Domain.Common.Enum.ExperimentLogStatus.Completed:
                    el.Complete();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextStatus), "State không hợp lệ.");
            }
        }
    }
}
