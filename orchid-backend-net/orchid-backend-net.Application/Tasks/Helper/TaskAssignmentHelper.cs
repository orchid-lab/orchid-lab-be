namespace orchid_backend_net.Application.Tasks.Helper
{
    public static class TaskAssignmentHelper
    {
        public static void UpdateTaskAssignmentOfTask(Domain.Entities.Tasks task, string? taskAssignmentId, string? sampleId, bool isForWholeExperimentLog)
        {
            if (string.IsNullOrWhiteSpace(taskAssignmentId))
                return;
            task.UpdateTaskAssignment(
                taskAssignmentId,
                sampleId,
                isForWholeExperimentLog
            );
        }

        public static void AddTaskAssignmentToTask(Domain.Entities.Tasks task, string? technicianId, string? sampleId, bool isForWholeExperimentLog)
        {
            if (string.IsNullOrWhiteSpace(technicianId))
                return;
            task.AddTaskAssignment(technicianId, sampleId, isForWholeExperimentLog);
        }
    }
}
