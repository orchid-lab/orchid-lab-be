namespace orchid_backend_net.API.Dto.ExperimentLog
{
    /// <summary>
    /// transfer dto for delete experiment log, include reason, conclusion, issue and recommendation
    /// </summary>
    /// <param name="Reason"></param>
    /// <param name="Conclusion"></param>
    /// <param name="Issue"></param>
    /// <param name="Recommendation"></param>
    public record DeleteExperimentLogDto(string? Reason, string Conclusion, string Issue, string Recommendation);
}
