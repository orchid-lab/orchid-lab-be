using MediatR;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.Common.Behaviours
{
    public class UserExistenceValidationBehaviour<TRequest, TResponse>(
        ICurrentUserService currentUserService,
        IUserRepository userRepository) 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            // Skip validation if no user is authenticated
            if (string.IsNullOrEmpty(currentUserService.UserId))
            {
                return await next();
            }

            // Skip for authentication endpoints (Login, Register, RefreshToken, Logout)
            var requestType = typeof(TRequest).Name;
            if (requestType.Contains("Login") ||
                requestType.Contains("Register") ||
                requestType.Contains("RefreshToken") ||
                requestType.Contains("Logout"))
            {
                return await next();
            }

            // Validate user exists and is not deleted
            var user = await userRepository.FindAsync(
                u => u.ID == currentUserService.UserId && u.DeletedDate == null,
                cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "Phiên đăng nhập không hợp lệ. Người dùng không tồn tại hoặc đã bị vô hiệu hóa. " +
                    "Vui lòng đăng nhập lại để tiếp tục.");
            }

            return await next();
        }
    }
}