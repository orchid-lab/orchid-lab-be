using MediatR;
namespace orchid_backend_net.Application.Disease.UseCase.SetDiseaseActive
{
    public record SetDiseaseActiveCommand(int Id, bool IsActive) : IRequest<string>;
}