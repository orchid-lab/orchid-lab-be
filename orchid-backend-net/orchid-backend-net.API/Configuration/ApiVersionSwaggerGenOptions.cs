using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace orchid_backend_net.API.Configuration
{
    /// <summary>
    /// configure dependency injection for swagger gen options with api versioning
    /// </summary>
    /// <param name="provider"></param>
    public class ApiVersionSwaggerGenOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider = provider;

        /// <summary>
        /// configure with swagger gen options
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions.OrderByDescending(o => o.ApiVersion))
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Orchid Lab API",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description = "This API version has been deprecated.";
            }
            return info;
        }
    }
}
