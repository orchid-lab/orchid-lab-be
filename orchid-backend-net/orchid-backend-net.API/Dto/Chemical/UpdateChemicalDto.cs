namespace orchid_backend_net.API.Dto.Chemical
{
    /// <summary>
    /// DTO for updating a chemical.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="Category"></param>
    /// <param name="Unit"></param>
    public record UpdateChemicalDto(
        string? Name,
        string? Description,
        string? Category,
        string? Unit);
}
