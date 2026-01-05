using MediatR;
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
                await mediator.Publish(domainEvent);
            }
        }
    }
}
