namespace orchid_backend_net.Application.Method.Dto.Method
{
    public record CreateMethodDto(
        string Name, 
        string? Description, 
        List<int> CreateMaterial);
}
