using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using orchid_backend_net.Application.Common.Interfaces;
using System.Security.Claims;

namespace orchid_backend_net.API.Service
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, ILogger<CurrentUserService> _logger) : ICurrentUserService
    {
        private readonly ClaimsPrincipal? _claimsPrincipal = httpContextAccessor?.HttpContext?.User;

        public string? UserId => _claimsPrincipal?.FindFirst(JwtClaimTypes.Subject)?.Value;
        public string? UserName => _claimsPrincipal?.FindFirst(JwtClaimTypes.Name)?.Value;
        public async Task<bool> AuthorizeAsync(string policy)
        {
            if (_claimsPrincipal == null) return false;
            _logger.LogInformation("User {UserName}", UserName);
            return (await authorizationService.AuthorizeAsync(_claimsPrincipal, policy)).Succeeded;
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            _logger.LogInformation("User {UserName}", UserName);
            return await Task.FromResult(_claimsPrincipal?.IsInRole(role) ?? false);
        }
    }
}
