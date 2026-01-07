using MediatR;

namespace orchid_backend_net.Application.Method.UseCase.CreateMethod
{
    public record CreateMethodCommand() : IRequest<string>;
}
