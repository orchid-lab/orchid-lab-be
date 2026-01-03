using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Exceptions;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.UpdateUser
{
    public class UpdateUserInformationCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UpdateUserInformationCommand(string id, string? name, string? email, string? phoneNumber)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }

    internal class UpdateUserCommandHandler(IUserRepository userRepository, ICurrentUserService currentUserService) : IRequestHandler<UpdateUserInformationCommand, string>
    {
        public async Task<string> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindAsync(u => u.ID.Equals(request.Id) && u.DeletedDate == null, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng");
            }
            user.Name = request.Name ?? user.Name;
            user.Email = request.Email ?? user.Email;
            user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            user.UpdatedBy = currentUserService.UserId;
            user.UpdatedDate = DateTime.UtcNow;
            userRepository.Update(user);
            return await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0 ? "Sửa đổi thành công" : "Sửa đổi thất bại";
        }
    }
}
