namespace orchid_backend_net.API.Dto.Disease
{
    public record UpdateDiseaseRequest(
        string Name,
        string Code,
        string? Description,
        string? OnnxClassName  
    );
}