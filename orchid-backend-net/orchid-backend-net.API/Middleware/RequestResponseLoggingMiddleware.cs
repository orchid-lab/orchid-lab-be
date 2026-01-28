using System.Diagnostics;
using System.Text;

namespace orchid_backend_net.API.Middleware;

/// <summary>
/// Middleware for detailed request and response logging with performance tracking
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization", "Cookie", "X-Api-Key", "Password"
    };

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // Log request
        await LogRequest(context);

        // Capture original response body stream
        var originalBodyStream = context.Response.Body;
        
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            // Log response
            await LogResponse(context, stopwatch.ElapsedMilliseconds);
            
            // Copy response back to original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task LogRequest(HttpContext context)
    {
        try
        {
            var request = context.Request;
            var logMessage = new StringBuilder();
            
            logMessage.AppendLine($"HTTP Request Information:");
            logMessage.AppendLine($"Method: {request.Method}");
            logMessage.AppendLine($"Path: {request.Path}");
            logMessage.AppendLine($"QueryString: {request.QueryString}");
            logMessage.AppendLine($"Headers: {GetSanitizedHeaders(request.Headers)}");
            
            if (request.ContentLength > 0)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;
                
                // Don't log sensitive request bodies (passwords, tokens, etc.)
                if (!request.Path.Value?.Contains("auth", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    logMessage.AppendLine($"Body: {SanitizeBody(bodyAsText)}");
                }
                else
                {
                    logMessage.AppendLine("Body: [REDACTED - Authentication Request]");
                }
            }
            
            _logger.LogDebug(logMessage.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log request details");
        }
    }

    private async Task LogResponse(HttpContext context, long elapsedMs)
    {
        try
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            
            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Debug;
            
            _logger.Log(logLevel, 
                "HTTP Response: Status={StatusCode}, Time={ElapsedMs}ms, Body={ResponseBody}", 
                response.StatusCode, 
                elapsedMs,
                text.Length > 1000 ? $"{text[..1000]}..." : text);
                
            // Log performance warning for slow requests
            if (elapsedMs > 3000)
            {
                _logger.LogWarning("Slow request detected: {Method} {Path} took {ElapsedMs}ms", 
                    context.Request.Method, 
                    context.Request.Path, 
                    elapsedMs);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log response details");
        }
    }

    private static string GetSanitizedHeaders(IHeaderDictionary headers)
    {
        var sanitized = headers
            .Where(h => !SensitiveHeaders.Contains(h.Key))
            .Select(h => $"{h.Key}={h.Value}")
            .ToList();
            
        return string.Join(", ", sanitized);
    }

    private static string SanitizeBody(string body)
    {
        // In production, implement more sophisticated sanitization
        // For now, truncate if too long
        return body.Length > 2000 ? $"{body[..2000]}..." : body;
    }
}
