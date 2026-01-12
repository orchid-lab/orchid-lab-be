using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Application.Method.Dto.Method
{
    public record CreateMethodDto(
        int StageDefinitionId,
        int Order,
        int DurationDays,
        List<int> CreateMaterial,
        List<int> CreateChemical);
}
