using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.DeleteUser
{
    public class DeleteUserCommand : IRequest<string>
    {
        public string Id { get; set; }
        public DeleteUserCommand(string id)
        {
            Id = id;
        }
    }

    internal class DeleteUserCommandHandler(IUserRepository userRepository, ICurrentUserService currentUserService) : IRequestHandler<DeleteUserCommand, string>
    {
        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(u => u.ID.Equals(request.Id) && u.DeletedDate == null, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng.");
            }
            user.DeletedBy = currentUserService.UserId;
            user.DeletedDate = DateTime.UtcNow.AddHours(7);
            return await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Xóa thành công." : "Xóa thất bại.";
        }
    }
}
