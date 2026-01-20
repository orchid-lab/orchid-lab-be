namespace orchid_backend_net.API.Dto.Material
{
    /// <summary>
    /// use this dto to transfer data when update material
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="Category"></param>
    /// <param name="Unit"></param>
    public record UpdateMaterialDto(string? Name, string? Description, string? Category, string? Unit);
}
