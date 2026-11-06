using Cortex.Mediator.Notifications;

namespace Veygo.Platform.Shared.Domain.Model.Events;

/// <summary>
///     Represents a domain event in the system.
/// </summary>
/// <remarks>
///     It extends <see cref="INotification" /> so domain events can be published through Cortex Mediator.
/// </remarks>
public interface IEvent : INotification
{
}
