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
using orchid_backend_net.Infrastructure.Service.PdfGenerator;
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
                options.Configuration = redisOptions.Configuration;
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
            services.Configure<GmailOptions>(options =>
            {
                options.ClientId = Environment.GetEnvironmentVariable("GMAIL_CLIENT_ID") ?? "";
                options.ClientSecret = Environment.GetEnvironmentVariable("GMAIL_CLIENT_SECRET") ?? "";
                options.RefreshToken = Environment.GetEnvironmentVariable("GMAIL_REFRESH_TOKEN") ?? "";
                options.Email = Environment.GetEnvironmentVariable("GMAIL_EMAIL") ?? "";
            });

            //gmail services
            //for local
            //services.Configure<GmailOptions>(configuration.GetSection("GmailOptions"));


            //Seed data generation
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrchidDbContext>();
                SeedDataGenerator.SeedAsync(dbContext).GetAwaiter().GetResult();
            }

            //Add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IElementRepositoty, ElementRepository>();
            services.AddScoped<IExperimentLogRepository, ExperimentLogRepository>();
            services.AddScoped<ILabRoomRepository, LabRoomRepository>();
            services.AddScoped<IMethodRepository, MethodRepository>();
            services.AddScoped<IReportRepository, RepostRepository>();
            services.AddScoped<ISampleRepository, SampleRepository>();
            services.AddScoped<ISeedlingAttributeRepository, SeedlingAttributeRepository>();
            services.AddScoped<ISeedlingRepository, SeedlingRepository>();
            services.AddScoped<ICharactersicticRepository, CharacteristicRepository>();
            services.AddScoped<IStageRepository, StageRepository>();
            services.AddScoped<ITaskAttributeRepository, TaskAttributeRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskAssignRepository, TaskAssignRepository>();
            services.AddScoped<IHybridizationRepository, HybridizationRepository>();
            services.AddScoped<ILinkedRepository, LinkedRepository>();
            services.AddScoped<ITissueCultureBatchRepository, TissueCultureBatchRepository>();
            services.AddScoped<IElementInStageRepository, ElementInStageRepository>();
            services.AddScoped<IReferentRepository, ReferentRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IImageUploaderService, CloudinaryImageUploaderService>();
            services.AddScoped<IOrchidAnalyzerService, OrchidAnalyzerService>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IDiseaseRepository, DiseaseRepository>();
            services.AddScoped<IInfectedSampleRepository, InfectedSampleRepository>();
            services.AddScoped<ITaskTemplatesRepository, TaskTemplateRepository>();
            services.AddScoped<ITaskTemplateDetailsRepository, TaskTemplateDetailRepository>();
            services.AddScoped<IReportAttributeRepository, ReportAttributeRepository>();
            services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();
            services.AddHostedService<Service.HostedService.ExperimentLogHostedService>();
            return services;
        }
    }
}