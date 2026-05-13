using MediatR;

namespace orchid_backend_net.Application.Disease.UseCase.CreateDisease
{
    public record CreateDiseaseCommand(
        string Name,
        string Code,
        string? Description,
        string? OnnxClassName
    ) : IRequest<string>;
}
