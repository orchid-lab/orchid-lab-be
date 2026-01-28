using Serilog.Context;

namespace orchid_backend_net.API.Middleware;

/// <summary>
/// Middleware to generate and track correlation IDs for each request
/// </summary>
public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);
        
        // Add to response headers for client tracking
        context.Response.Headers.Append(CorrelationIdHeaderName, correlationId);
        
        // Add to Serilog LogContext for all logs in this request
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogDebug("Request started: {Method} {Path} - CorrelationId: {CorrelationId}", 
                context.Request.Method, 
                context.Request.Path, 
                correlationId);
            
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogDebug("Request completed: {Method} {Path} - Status: {StatusCode} - CorrelationId: {CorrelationId}", 
                    context.Request.Method, 
                    context.Request.Path, 
                    context.Response.StatusCode,
                    correlationId);
            }
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        // Check if client sent correlation ID
        if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId))
        {
            return correlationId.ToString();
        }

        // Generate new correlation ID
        return Guid.NewGuid().ToString();
    }
}
