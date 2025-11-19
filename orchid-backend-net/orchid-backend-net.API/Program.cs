using orchid_backend_net.API.Configuration;
using orchid_backend_net.API.Filters;
using orchid_backend_net.API.Middleware;
using orchid_backend_net.Application;
using orchid_backend_net.Infrastructure;
using orchid_backend_net.Infrastructure.Service.GmailSettings;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//serilog service mf
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

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

//optimization
builder.Services.AddMemoryCache();

builder.Services.AddLogging(opt =>
{
    opt.ClearProviders(); // Clear default providers
    opt.AddConsole(); // Add console logging
    opt.AddDebug(); // Add debug logging
    opt.SetMinimumLevel(LogLevel.Information); // Set minimum log level
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();

app.UseCorsPolicy();

app.UseAuthorization();

app.MapControllers();

app.Run();
