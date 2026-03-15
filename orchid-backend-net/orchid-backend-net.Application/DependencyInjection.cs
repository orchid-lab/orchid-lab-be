using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using orchid_backend_net.Application.Common.Behaviours;
using orchid_backend_net.Application.Common.Validation;
using orchid_backend_net.Application.ExperimentLog.Helper;
using orchid_backend_net.Application.ExperimentLog.Helper.CreateExperimentLogHelperInjection;
using orchid_backend_net.Application.MonitoringLog.UseCase.CreateMonitoringLog;
using System.Reflection;

namespace orchid_backend_net.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), lifetime: ServiceLifetime.Transient);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
                cfg.AddOpenBehavior(typeof(AuthorizationBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
                cfg.AddOpenBehavior(typeof(UserExistenceValidationBehaviour<,>));
            });

            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.AddScoped<IValidationProvider, ValidationProvider>();
            services.AddTransient<ExperimentLogSeedTask>();
            services.AddTransient<CreateExperimentLogServices>();
            services.AddTransient<CreateExperimentLogRepositories>();
            services.AddTransient<CreateMonitoringLogRepository>();
            return services;
        }
    }
}
