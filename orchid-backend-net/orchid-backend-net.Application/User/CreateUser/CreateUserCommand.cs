using MediatR;
using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.User.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required int RoleID { get; set; }

        public CreateUserCommand(string name, string email, string phoneNumber, int roleID, string password)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            RoleID = roleID;
            Password = password;
        }

        public CreateUserCommand()
        {
        }
    }

    internal class CreateUserCommandHandler(IUserRepository userRepository,
        ICurrentUserService currentUserService, IEmailSender emailSender) : IRequestHandler<CreateUserCommand, string>
    {
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Users user = new()
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                RoleID = request.RoleID,
                Status = true,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Create_by = currentUserService.UserId,
                Create_date = DateTime.UtcNow.AddHours(7),
            };

            var templatePath = Path.Combine(AppContext.BaseDirectory, "User", "EmailTemplate.html");
            var emailBody = await File.ReadAllTextAsync(templatePath);
            emailBody = emailBody.Replace("{UserName}", user.Name)
                .Replace("{UserEmail}", user.Email)
                .Replace("{UserPassword}", request.Password);
            await emailSender.SendEmailAsync(user.Email, "Thông báo tài khoản hệ thống OrchidLab", emailBody);
            userRepository.Add(user);
            return await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? $"Created User with ID: {user.ID}"
                : "Failed to create User.";
        }
    }
}
