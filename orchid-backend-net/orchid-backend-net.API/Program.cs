using orchid_backend_net.API.Configuration;
using orchid_backend_net.API.Filters;
using orchid_backend_net.API.Middleware;
using orchid_backend_net.Application;
using orchid_backend_net.Infrastructure;
using orchid_backend_net.Infrastructure.Service;
using orchid_backend_net.Infrastructure.Service.GmailSettings;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Serilog for comprehensive logging (100% debug mode)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "OrchidLab-Backend")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{CorrelationId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30) // Keep 30 days of logs
    .WriteTo.File(
        path: "Logs/errors/error-.txt",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{CorrelationId}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 90) // Keep 90 days of error logs
    .CreateLogger();

Log.Information("Starting OrchidLab Backend API in {Environment} mode", builder.Environment.EnvironmentName);

builder.Host.UseSerilog();

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Register application and infrastructure services
builder.Services.AddApplication(builder.Configuration);
builder.Services.ConfigureApplicationSecurity(builder.Configuration);
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureProblemDetails();
builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigurationCors();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks();

//optimization
builder.Services.AddMemoryCache();

// Configure logging levels based on environment
builder.Services.AddLogging(opt =>
{
    opt.ClearProviders();
    opt.AddSerilog(); // Use Serilog for all logging
    
    // Set minimum log level based on environment
    if (builder.Environment.IsDevelopment())
    {
        opt.SetMinimumLevel(LogLevel.Debug); // Debug mode in development
    }
    else if (builder.Environment.IsProduction())
    {
        opt.SetMinimumLevel(LogLevel.Information); // Information in production
    }
    
    // Configure specific log levels for Microsoft components
    opt.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    opt.AddFilter("Microsoft.AspNetCore.Hosting", LogLevel.Information);
});

// Configure the HTTP request pipeline.
Log.Information("Configuring HTTP request pipeline");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Log.Information("Swagger UI enabled for Development environment");
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Log.Information("Swagger UI enabled for Production environment");
}

app.UseHttpsRedirection();

app.UseRouting();

// Add correlation ID middleware early in the pipeline
app.UseMiddleware<CorrelationIdMiddleware>();

// Add request/response logging middleware (only in development for detailed logs)
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    Log.Information("Request/Response logging middleware enabled for Development");
}

app.UseCorsPolicy();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RateLimitingMiddleware>();

Log.Information("Mapping controllers and hubs");
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHealthChecks("/health");

Log.Information("OrchidLab Backend API is ready to accept requests");
await app.RunAsync();
