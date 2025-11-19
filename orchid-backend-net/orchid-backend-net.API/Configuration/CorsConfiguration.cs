namespace orchid_backend_net.API.Configuration
{
    public static class CorsConfiguration
    {
        public static IServiceCollection ConfigurationCors(this IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            return services;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        {
            
            return app.UseCors("CorsPolicy");
        }
    }
}
