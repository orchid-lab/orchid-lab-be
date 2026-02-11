using MediatR;
using orchid_backend_net.Domain.Common.Interfaces;

namespace orchid_backend_net.Application.Common.Events
{
    /// <summary>
    /// Generic wrapper that bridges Domain Events to MediatR INotification.
    /// Supports all event types: notification push, seed task, sample generation, etc.
    /// </summary>
    public record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent)
        : INotification where TDomainEvent : IDomainEvent;
}