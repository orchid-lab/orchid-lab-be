using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace orchid_backend_net.Infrastructure.Service
{
    public sealed class LoggingFilter(ILogger<LoggingFilter> logger) : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            var hub = invocationContext.Hub?.GetType().Name ?? "UnknownHub";
            var method = invocationContext.HubMethodName;
            var connectionId = invocationContext.Context?.ConnectionId ?? "unknown";
            var userId = invocationContext.Context?.UserIdentifier ?? "anonymous";
            var argsCount = invocationContext.HubMethodArguments?.Count ?? 0;

            logger.LogInformation("SignalR invoking: Hub={Hub}, Method={Method}, UserId={UserId}, ConnectionId={ConnectionId}, ArgsCount={ArgsCount}",
                hub, method, userId, connectionId, argsCount);

            try
            {
                var result = await next(invocationContext);
                logger.LogInformation("SignalR invoked: Hub={Hub}, Method={Method}, UserId={UserId}, ConnectionId={ConnectionId} succeeded",
                    hub, method, userId, connectionId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SignalR invocation error: Hub={Hub}, Method={Method}, UserId={UserId}, ConnectionId={ConnectionId}",
                    hub, method, userId, connectionId);

                var contextualMessage = $"SignalR invocation failed. Hub={hub}, Method={method}, UserId={userId}, ConnectionId={connectionId}";
                throw new HubException(contextualMessage, ex);
            }
        }

        public async ValueTask OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, ValueTask> next)
        {
            var userId = context.Context.UserIdentifier ?? "anonymous";
            logger.LogDebug("SignalR filter OnConnected: UserId={UserId}, ConnectionId={ConnectionId}", userId, context.Context.ConnectionId);
            await next(context);
        }

        public async ValueTask OnDisconnectedAsync(HubLifetimeContext context, Exception? exception, Func<HubLifetimeContext, Exception?, ValueTask> next)
        {
            var userId = context.Context.UserIdentifier ?? "anonymous";
            logger.LogDebug("SignalR filter OnDisconnected: UserId={UserId}, ConnectionId={ConnectionId}", userId, context.Context.ConnectionId);
            await next(context, exception);
        }
    }
}
