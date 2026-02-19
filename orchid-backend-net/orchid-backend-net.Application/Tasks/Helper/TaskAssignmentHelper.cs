using orchid_backend_net.Domain.Common.Enum;

namespace orchid_backend_net.Application.Tasks.Helper
{
    public static class TaskAssignmentHelper
    {
        public static void ReassignTaskAssignmentToTask(
            Domain.Entities.Tasks task,
            TaskTargetType? newTargetType,
            string? newTargetId,
            DateTime? expectedEndDate,
            DateTime? endDate)
        {
            task.ReassignTaskTarget(
                newTargetType,
                newTargetId,
                expectedEndDate,
                endDate
            );
        }

        public static void AddTaskAssignmentToTask(
            Domain.Entities.Tasks task,
            string technicianId, 
            TaskTargetType targetType, 
            string targetId, 
            DateTime expectedEndDate, 
            DateTime startDate,
            bool isSeeding = false)
        {
            if (string.IsNullOrWhiteSpace(technicianId))
                return;
            task.AddTaskAssignment(technicianId, targetType, targetId, expectedEndDate, startDate, isSeeding);
        }
    }
}
