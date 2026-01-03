using MediatR;
using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Authentication.Register
{
    public class RegisterCommand(string name, string email, string phoneNumber, int roleID) : IRequest<string>
    {
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string PhoneNumber { get; set; } = phoneNumber;
        public int RoleID { get; set; } = roleID;
    }

    internal class RegisterCommandHandler(IUserRepository userRepository,
      ICurrentUserService currentUserService, IEmailSender emailSender) : IRequestHandler<RegisterCommand, string>
    {
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if(request.RoleID == 1) // Admin role
            {
                var isCurrentUserAdmin = await currentUserService.IsInRoleAsync("Admin");
                if (!isCurrentUserAdmin)
                {
                    return "Bạn không có quyền tạo tài khoản Admin.";
                }
            }

            Users user = new()
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                RoleID = request.RoleID,
                Password = BCrypt.Net.BCrypt.HashPassword("123@123a"),
                CreatedBy = currentUserService.UserId,
                CreatedDate = TimeZoneHelper.VietnamTimeNow,
            };

            var emailBody = await LoadEmailTemplateAsync(cancellationToken);

            emailBody = emailBody.Replace("{UserName}", user.Name)
                .Replace("{UserEmail}", user.Email)
                .Replace("{UserPassword}", "123@123a");
            await emailSender.SendEmailAsync(user.Email, "Thông báo tài khoản hệ thống OrchidLab", emailBody);
            userRepository.Add(user);
            return await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? $"Tạo tài khoản thành công với id: {user.ID}"
                : "Tạo tài khoản thất bại.";
        }

        private static async Task<string> LoadEmailTemplateAsync(CancellationToken cancellationToken)
        {
            var assembly = typeof(RegisterCommandHandler).Assembly;
            var resourceName =
                "orchid_backend_net.Application.Authentication.EmailTemplate.html";

            await using var stream =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException("Email template not found");

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken);
        }
    }
}
