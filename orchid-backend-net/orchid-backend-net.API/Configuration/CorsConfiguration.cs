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
                        .WithOrigins(
                        "http://localhost:3000",
                        "https://client.tissuex.me",
                        "http://localhost:7059",  
                        "https://localhost:7059"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            return services;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        {
            
            return app.UseCors("CorsPolicy");
        }
    }
}
