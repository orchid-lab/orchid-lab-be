using CloudinaryDotNet;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
using Polly;
using Polly.Extensions.Http;
using System.Net;

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

                var cloudinary = new Cloudinary(account);
                cloudinary.Api.Timeout = 5000;
                cloudinary.Api.Secure = true;
                return cloudinary;
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
            services.AddSingleton<LoggingFilter>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IImageUploaderService, CloudinaryImageUploaderService>();
            services.AddScoped<IDateTimeProvider, VietNamDateTimeProvider>();
            services.AddScoped<IHubnotificationService, HubNotificationService>();
            services.AddScoped<INotificationPushService, NotificationPushService>();
            services.AddScoped<IOrchidAnalyzerService, OrchidAnalyzerService>();
            //Add repositories

            //for config and safe procedure module
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<ISafeProcedureRepository, SafeProcedureRepository>();

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
            services.AddScoped<ILabRoomRepository, LabRoomRepository>();
            services.AddScoped<ISampleStageDefinitionRepository, SampleStageDefinitionRepository>();
            services.AddScoped<IStageRequirementDefinitionRepository, StageRequirementDefinitionRepository>();
            services.AddScoped<ISampleStageRepository, SampleStageRepository>();
            services.AddScoped<IMethodStageDefinitionRepository, MethodStageDefinitionRepository>();

            //for monitoring log module
            services.AddScoped<IMonitoringLogRepository, MonitoringLogRepository>();    
            services.AddScoped<IDiseaseRepository, DiseaseRepository>();
            services.AddScoped<IAnalyticResultRepository, AnalyticResultRepository>();

            //for image module
            services.AddScoped<IImageRepository, ImageRepository>();

            //signalR
            services.AddSignalR(opt =>
            {
                opt.AddFilter<LoggingFilter>();
                opt.EnableDetailedErrors = true;
            });


            //httpclient for some service required 3rd parties with tune handler + polly
            services.AddHttpClient<OrchidAnalyzerService>((sp, client) =>
            {
                var pythonApiUrl = configuration["OrchidAnalyzer:PythonApiUrl"];
                if(string.IsNullOrWhiteSpace(pythonApiUrl))
                    throw new InvalidOperationException("OrchidAnalyzer:PythonApiUrl is not configured.");

                client.BaseAddress = new Uri(pythonApiUrl);
                client.Timeout = TimeSpan.FromSeconds(5); // tight per-request timeout, maybe tune later
                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");

                //prefer http/2 for all request
                client.DefaultRequestVersion = HttpVersion.Version20;
                client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(2),   // rotate connections
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                    MaxConnectionsPerServer = 100,                        // tune based on load
                    EnableMultipleHttp2Connections = true,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                    AllowAutoRedirect = false,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(30),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(5),
                    KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always
                };
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetTimeOutPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
               .HandleTransientHttpError()
               .OrResult(r => (int)r.StatusCode >= 500)
               .WaitAndRetryAsync(3, retryAttempt =>
                   TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)),
                   onRetry: (outcome, delay, attempt, ctx) => { /* add logging if needed */ });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetTimeOutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
               .HandleTransientHttpError()
               .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}