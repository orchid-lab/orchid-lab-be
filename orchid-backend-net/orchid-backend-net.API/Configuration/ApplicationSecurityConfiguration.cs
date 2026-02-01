using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using orchid_backend_net.API.Service;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.API.Configuration
{
    /// <summary>
    /// appilication security configuration extension
    /// </summary>
    public static class ApplicationSecurityConfiguration
    {
        /// <summary>
        /// configure application security
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureApplicationSecurity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<JwtService>();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = "https://net-api.tissuex.me/",
                        ValidAudience = "api",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OrchidLabManagementSystemsDotNetApi")),
                        NameClaimType = ClaimTypes.NameIdentifier,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var path = context.HttpContext.Request.Path;
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/hubs/notifications"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(ConfigureAuthorization);

            return services;
        }

        /// <summary>
        /// Configures authorization policies and options for the application.
        /// </summary>
        /// <param name="options">The authorization options to be configured. Cannot be null.</param>
        private static void ConfigureAuthorization(AuthorizationOptions options)
        {

        }
    }
}
