using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Domain.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class DomainEventDispatcher(IMediator mediator)
        : IDomainEventDispatcher
    {
        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var notificationType = typeof(DomainEventNotification<>)
                    .MakeGenericType(domainEvent.GetType());

                var notification = Activator.CreateInstance(notificationType, domainEvent)
                    ?? throw new InvalidOperationException(
                        $"Could not create DomainEventNotification for {domainEvent.GetType().Name}");

                await mediator.Publish(notification);
            }
        }
    }
}
