using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;
using orchid_backend_net.Infrastructure.Repository;
using orchid_backend_net.Infrastructure.Service;
using orchid_backend_net.Infrastructure.Service.CloudinarySettings;
using orchid_backend_net.Infrastructure.Service.GmailSettings;
using orchid_backend_net.Infrastructure.Service.RedisSettings;

namespace orchid_backend_net.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //database context
            services.AddDbContext<OrchidDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Server"), b =>
                {
                    b.MigrationsAssembly(typeof(OrchidDbContext).Assembly.FullName);
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
                options.UseLazyLoadingProxies();
            });
            services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<OrchidDbContext>());

            //redis cache
            services.AddStackExchangeRedisCache(options =>
            {
                var redisOptions = configuration.GetSection("Redis").Get<RedisOptions>();
                options.Configuration = redisOptions!.Configuration;
                options.InstanceName = redisOptions.InstanceName;
            });

            //cloudinary service to store images
            services.Configure<CloudinaryOptions>(configuration.GetSection("Cloudinary"));
            services.AddSingleton<Cloudinary>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CloudinaryOptions>>().Value;
                var account = new Account(
                    options.CloudName,
                    options.ApiKey,
                    options.ApiSecret
                );
                return new Cloudinary(account);
            });

            //refactor: all configure must be in programcs to take the appsettings not in here

            //gmail services 
            //only use for production stage
            //when use in local please comment these lines 
            services.Configure<GmailOptions>(configuration.GetSection("Gmail"));


            //Seed data generation and check migration
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrchidDbContext>();
                dbContext.Database.Migrate();
                SeedDataGenerator.SeedAsync(dbContext)
                                 .GetAwaiter()
                                 .GetResult();
            }

            //service
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IImageUploaderService, CloudinaryImageUploaderService>();
            //Add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ISeedlingRepository, SeedlingRepository>();
            services.AddScoped<ISeedlingTraitRepository, SeedlingTraitRepository>();
            services.AddScoped<ICharacteristicRepository, CharacteristicRepository>();

            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddScoped<IStageRepository, StageRepository>();
            return services;
        }
    }
}