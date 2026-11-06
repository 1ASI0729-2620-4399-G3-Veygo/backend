namespace Veygo.Platform.Booking.Interfaces.Rest.Resources;

/// <summary>
///     Booking returned by the API. <c>startDate</c>/<c>endDate</c> use <c>yyyy-MM-dd</c>;
///     <c>status</c> is pending, confirmed, rejected, cancelled or completed; <c>paymentMethod</c> is card or yape.
/// </summary>
public record BookingResource(
    int Id,
    int VehicleId,
    int RenterId,
    int OwnerId,
    string StartDate,
    string EndDate,
    decimal PricePerDay,
    decimal TotalPrice,
    string Status,
    string PaymentMethod,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? ConfirmedAt);