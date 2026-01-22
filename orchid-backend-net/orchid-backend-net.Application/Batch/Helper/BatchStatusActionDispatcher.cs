namespace orchid_backend_net.Application.Batch.Helper
{
    public static class BatchStatusActionDispatcher
    {
        public static void Dispatch(
            Domain.Entities.Batches batch,
            Domain.Common.Enum.BatchStatus nextStatus,
            string currentUserId)
        {
            switch (nextStatus)
            {
                case Domain.Common.Enum.BatchStatus.Cleaning:
                    batch.FinishBatching(currentUserId);
                    break;
                case Domain.Common.Enum.BatchStatus.Ready:
                    batch.CompleteCleaning(currentUserId);
                    break;
                case Domain.Common.Enum.BatchStatus.Maintenance:
                    batch.SetToMaintenance(currentUserId);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Không hỗ trợ chuyển sang trạng thái {nextStatus}");
            }
        }
    }
}
