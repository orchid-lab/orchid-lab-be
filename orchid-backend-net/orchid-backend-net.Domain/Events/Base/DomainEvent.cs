using orchid_backend_net.Domain.Common.Interfaces;

namespace orchid_backend_net.Domain.Events.Base
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }
}
