using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace orchid_backend_net.API.Filters
{
    /// <summary>
    /// Adds a security requirement to OpenAPI operations that are protected by the Authorize attribute.
    /// </summary>
    /// <remarks>This operation filter is typically used with Swashbuckle or similar OpenAPI/Swagger
    /// generators to ensure that endpoints requiring authorization are properly documented with security requirements.
    /// It inspects controller actions and their declaring types for the presence of the Authorize attribute and, if
    /// found, adds a security scheme reference (such as 'Bearer') to the operation's security requirements. This
    /// enables tools like Swagger UI to prompt for authentication when testing secured endpoints.</remarks>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Adds a Bearer security requirement to the specified OpenAPI operation if authorization is required.
        /// </summary>
        /// <remarks>This method is typically used in Swagger or Swashbuckle filters to indicate that an
        /// operation requires JWT Bearer authentication. If the operation does not require authorization, no changes
        /// are made.</remarks>
        /// <param name="operation">The OpenAPI operation to which the security requirement will be applied.</param>
        /// <param name="context">The context containing information about the current operation and API description.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!HasAuthorize(context))
            {
                return;
            }
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }] = []
            });
        }

        private static bool HasAuthorize(OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
            {
                return true;
            }
            return context.MethodInfo.DeclaringType != null
                && context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        }
    }
}
