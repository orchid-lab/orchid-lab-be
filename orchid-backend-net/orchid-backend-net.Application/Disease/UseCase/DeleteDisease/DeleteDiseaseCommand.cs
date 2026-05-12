using MediatR;

namespace orchid_backend_net.Application.Disease.UseCase.DeleteDisease
{
    public record DeleteDiseaseCommand(int Id) : IRequest<string>;
}
