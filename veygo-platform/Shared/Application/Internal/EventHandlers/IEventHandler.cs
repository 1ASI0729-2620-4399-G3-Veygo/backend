using Cortex.Mediator.Notifications;
using Veygo.Platform.Shared.Domain.Model.Events;

namespace Veygo.Platform.Shared.Application.Internal.EventHandlers;

/// <summary>
///     Handles a domain event published through Cortex Mediator.
/// </summary>
/// <typeparam name="TEvent">The domain event type.</typeparam>
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}
