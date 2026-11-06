using Veygo.Platform.Booking.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Model.Events;

namespace Veygo.Platform.Booking.Domain.Model.Events;

/// <summary>
///     Raised when the owner confirms or rejects a booking, or when a booking is cancelled or completed.
/// </summary>
/// <param name="BookingId">The booking id.</param>
/// <param name="VehicleId">The booked vehicle.</param>
/// <param name="RenterId">The renter.</param>
/// <param name="OwnerId">The owner.</param>
/// <param name="Status">The new status.</param>
public record BookingStatusChangedEvent(int BookingId, int VehicleId, int RenterId, int OwnerId, EBookingStatus Status)
    : IEvent;