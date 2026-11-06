using Veygo.Platform.Booking.Domain.Model.Events;
using Veygo.Platform.Shared.Application.Internal.EventHandlers;

namespace Veygo.Platform.Notification.Application.Internal.EventHandlers;

/// <summary>
///     Reacts to <see cref="BookingStatusChangedEvent" /> raised by the Booking context.
/// </summary>
/// <remarks>
///     It runs after the booking was saved (see <c>DispatchDomainEventsInterceptor</c>), so the booking id is
///     already assigned. For now it only logs the event: in Sprint 2 the web application creates the
///     notifications itself (POST /api/v1/notifications), so creating them here too would duplicate them.
/// </remarks>
public class BookingStatusChangedEventHandler(ILogger<BookingStatusChangedEventHandler> logger)
    : IEventHandler<BookingStatusChangedEvent>
{
    /// <inheritdoc />
    public Task Handle(BookingStatusChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Booking {BookingId} of vehicle {VehicleId} changed to {Status} (renter {RenterId}, owner {OwnerId})",
            domainEvent.BookingId, domainEvent.VehicleId, domainEvent.Status, domainEvent.RenterId,
            domainEvent.OwnerId);
        return Task.CompletedTask;
    }
}
