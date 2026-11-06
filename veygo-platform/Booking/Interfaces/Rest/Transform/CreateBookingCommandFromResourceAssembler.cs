using Veygo.Platform.Booking.Domain.Model.Commands;
using Veygo.Platform.Booking.Domain.Model.ValueObjects;
using Veygo.Platform.Booking.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Booking.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateBookingCommand" /> from a <see cref="CreateBookingResource" />.
/// </summary>
public static class CreateBookingCommandFromResourceAssembler
{
    public static CreateBookingCommand ToCommandFromResource(CreateBookingResource resource) =>
        new(resource.VehicleId,
            resource.RenterId,
            DateValueAssembler.ToDate(resource.StartDate, "startDate"),
            DateValueAssembler.ToDate(resource.EndDate, "endDate"),
            string.IsNullOrWhiteSpace(resource.PaymentMethod)
                ? EPaymentMethod.Card
                : EnumValueAssembler.ToEnum<EPaymentMethod>(resource.PaymentMethod, "paymentMethod"));
}