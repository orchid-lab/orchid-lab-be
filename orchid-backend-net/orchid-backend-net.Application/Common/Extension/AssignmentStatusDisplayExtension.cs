namespace orchid_backend_net.Application.Common.Extension
{
    public static class AssignmentStatusDisplayExtension
    {
        public static string ToDisplayText(this Domain.Common.Enum.TaskTargetType taskTargetType)
        {
            return taskTargetType switch
            {
                Domain.Common.Enum.TaskTargetType.Sample => "cho mẫu thí nghiệm.",
                Domain.Common.Enum.TaskTargetType.ExperimentLog => "cho nhật ký thí nghiệm.",
                _ => "không xác định."
            };
        }
    }
}
