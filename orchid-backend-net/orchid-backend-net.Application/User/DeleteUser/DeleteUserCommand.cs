using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.DeleteUser
{
    public class DeleteUserCommand(string id) : IRequest<string>, ICommand
    {
        public string Id { get; set; } = id;
    }

    internal class DeleteUserCommandHandler(IUserRepository userRepository, ICurrentUserService currentUserService) : IRequestHandler<DeleteUserCommand, string>
    {
        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(x => x.ID.Equals(request.Id) && x.Status, cancellationToken);
            user.Status = false;
            user.Delete_date = DateTime.UtcNow;
            user.Delete_by = currentUserService.UserName;
            userRepository.Update(user);
            return await userRepository.UnitOfWork.SaveChangesAsync() > 0
                ? "Delete user successfully"
                : "Delete user failed";
        }
    }
}
