using Veygo.Platform.Booking.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="BookingResource" /> from a booking.
/// </summary>
public static class BookingResourceFromEntityAssembler
{
    public static BookingResource ToResourceFromEntity(BookingAggregate entity) =>
        new(entity.Id,
            entity.VehicleId,
            entity.RenterId,
            entity.OwnerId,
            DateValueAssembler.ToValue(entity.StartDate),
            DateValueAssembler.ToValue(entity.EndDate),
            entity.PricePerDay,
            entity.TotalPrice,
            EnumValueAssembler.ToValue(entity.Status),
            EnumValueAssembler.ToValue(entity.PaymentMethod),
            entity.CreatedAt,
            entity.ConfirmedAt);
}