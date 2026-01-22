using orchid_backend_net.Domain.Events.Base;

namespace orchid_backend_net.Domain.Events.ExperimentLogEvents
{
    public record ExperimentLogSampleGenerationRequired(
        string ExperimentLogId,
        int MethodStageId,
        int StageOrder,
        int ExpectedSampleCount,
        string TechnicianId)
        : DomainEvent;
}
