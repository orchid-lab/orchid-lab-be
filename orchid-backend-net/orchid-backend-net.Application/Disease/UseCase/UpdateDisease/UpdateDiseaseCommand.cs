using MediatR;

namespace orchid_backend_net.Application.Disease.UseCase.UpdateDisease
{
    public record UpdateDiseaseCommand(
        int Id,
        string Name,
        string Code,
        string? Description,
        string? OnnxClassName  
    ) : IRequest<string>;
}