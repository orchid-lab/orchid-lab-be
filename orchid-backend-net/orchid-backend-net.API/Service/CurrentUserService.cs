using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using orchid_backend_net.Application.Common.Interfaces;
using System.Security.Claims;

namespace orchid_backend_net.API.Service
{
    /// <summary>
    /// this service use to get the current user has login and use api
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="authorizationService"></param>
    /// <param name="_logger"></param>
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, ILogger<CurrentUserService> _logger) : ICurrentUserService
    {
        private readonly ClaimsPrincipal? _claimsPrincipal = httpContextAccessor?.HttpContext?.User;

        /// <summary>
        /// Id of current user using api and service
        /// </summary>
        public string? UserId =>
                    _claimsPrincipal?.FindFirst(JwtClaimTypes.Subject)?.Value
                    ?? _claimsPrincipal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? _claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        /// <summary>
        /// Name of current user using api and service
        /// </summary>
        public string? UserName => _claimsPrincipal?.FindFirst(JwtClaimTypes.Name)?.Value;
        /// <summary>
        /// this method is using to check if the current user has been authorized in system or not
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public async Task<bool> AuthorizeAsync(string policy)
        {
            if (_claimsPrincipal == null) return false;
            _logger.LogInformation("User {UserName}", UserName);
            return (await authorizationService.AuthorizeAsync(_claimsPrincipal, policy)).Succeeded;
        }
        /// <summary>
        /// this method is using to check if the current user role can use the service in system or not
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(string role)
        {
            _logger.LogInformation("User {UserName}", UserName);
            return await Task.FromResult(_claimsPrincipal?.IsInRole(role) ?? false);
        }
    }
}
