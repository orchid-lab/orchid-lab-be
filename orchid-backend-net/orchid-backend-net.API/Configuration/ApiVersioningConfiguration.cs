using Asp.Versioning;

namespace orchid_backend_net.API.Configuration
{
    /// <summary>
    /// Configuration for api version
    /// Seperated with DI
    /// </summary>
    public static class ApiVersioningConfiguration
    {
        /// <summary>
        /// Config API Version with the following of swashbuckle
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}
