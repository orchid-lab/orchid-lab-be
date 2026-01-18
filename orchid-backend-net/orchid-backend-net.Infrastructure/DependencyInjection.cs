using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.IRepositories;
using orchid_backend_net.Infrastructure.Persistence;
using orchid_backend_net.Infrastructure.Provider;
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
            //event dispatcher for db context
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            //db context connection string
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
                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                    SeedDataGenerator.SeedAsync(dbContext)
                                     .GetAwaiter()
                                     .GetResult();
                }
            }

            //service
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IImageUploaderService, CloudinaryImageUploaderService>();
            services.AddScoped<IDateTimeProvider, VietNamDateTimeProvider>();
            services.AddScoped<IHubnotificationService, HubNotificationService>();

            //Add repositories

            //for notification module
            services.AddScoped<INotificationRepository, NotificationRepository>();

            //for user module
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();

            //for seedling module
            services.AddScoped<ISeedlingRepository, SeedlingRepository>();
            services.AddScoped<ISeedlingTraitRepository, SeedlingTraitRepository>();
            services.AddScoped<ICharacteristicRepository, CharacteristicRepository>();

            //for task module
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IChemicalsRepository, ChemicalRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IStageDefinitionRepository, StageDefinitionRepository>();

            //for method and experiment log module
            services.AddScoped<IMethodRepository, MethodRepository>();
            services.AddScoped<ISampleRequirementDefinitionRepository, SampleRequirementRepository>();
            services.AddScoped<ISampleRepository, SampleRepository>();
            services.AddScoped<IExperimentLogRepository, ExperimentLogRepository>();
            services.AddScoped<IBatchesRepository, BatchesRepository>();

            //signalR
            services.AddSignalR();
            return services;
        }
    }
}