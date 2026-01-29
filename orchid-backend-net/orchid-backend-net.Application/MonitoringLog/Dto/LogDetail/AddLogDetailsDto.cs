namespace orchid_backend_net.Application.MonitoringLog.Dto.LogDetail
{
    public record AddLogDetailsDto(
        string StageRequirementDefinitionId,
        decimal MeasuredValue);
}
