using Veygo.Platform.Booking.Domain.Model.Commands;
using Veygo.Platform.Booking.Domain.Model.ValueObjects;
using Veygo.Platform.Booking.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Booking.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an <see cref="UpdateBookingStatusCommand" /> from an <see cref="UpdateBookingStatusResource" />.
/// </summary>
public static class UpdateBookingStatusCommandFromResourceAssembler
{
    public static UpdateBookingStatusCommand ToCommandFromResource(int bookingId,
        UpdateBookingStatusResource resource) =>
        new(bookingId, EnumValueAssembler.ToEnum<EBookingStatus>(resource.Status, "status"));
}