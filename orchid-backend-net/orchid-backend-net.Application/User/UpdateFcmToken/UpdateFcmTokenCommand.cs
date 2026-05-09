using MediatR;

namespace orchid_backend_net.Application.User.UpdateFcmToken
{
    public record UpdateFcmTokenCommand(string UserId, string FcmToken) : IRequest<string>;
}
