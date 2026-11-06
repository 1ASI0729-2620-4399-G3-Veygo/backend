using Veygo.Platform.Booking.Domain.Model.Commands;
using Veygo.Platform.Booking.Domain.Model.Events;
using Veygo.Platform.Booking.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Model.Aggregates.Internal;
using Veygo.Platform.Shared.Domain.Model.Entities;

namespace Veygo.Platform.Booking.Domain.Model.Aggregates;

/// <summary>
///     Booking aggregate root: a renter rents a vehicle of an owner for a period.
/// </summary>
/// <remarks>
///     The period goes from <see cref="StartDate" /> (pick-up) to <see cref="EndDate" /> (return).
///     Rented days are the nights between both dates, with a minimum of one day.
/// </remarks>
public class Booking : AggregateRoot, IAuditableEntity
{
    /// <summary>
    ///     Default constructor required by EF Core.
    /// </summary>
    protected Booking()
    {
    }

    /// <summary>
    ///     Creates a pending booking for the given vehicle terms.
    /// </summary>
    /// <param name="command">The booking request.</param>
    /// <param name="terms">The rental terms of the vehicle (owner and price).</param>
    /// <exception cref="ArgumentException">When a domain invariant is violated.</exception>
    public Booking(CreateBookingCommand command, VehicleRentalTerms terms)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(terms);
        if (command.VehicleId != terms.VehicleId)
            throw new ArgumentException("The rental terms do not belong to the requested vehicle.", nameof(terms));
        if (command.RenterId <= 0) throw new ArgumentException("A booking must reference a renter.", nameof(command));
        if (command.RenterId == terms.OwnerId)
            throw new ArgumentException("Owners cannot book their own vehicles.", nameof(command));
        if (command.EndDate < command.StartDate)
            throw new ArgumentException("The return date must be on or after the pick-up date.", nameof(command));
        if (terms.PricePerDay <= 0)
            throw new ArgumentException("The price per day must be greater than zero.", nameof(terms));

        VehicleId = command.VehicleId;
        RenterId = command.RenterId;
        OwnerId = terms.OwnerId;
        StartDate = command.StartDate;
        EndDate = command.EndDate;
        PricePerDay = terms.PricePerDay;
        TotalPrice = PricePerDay * Days;
        PaymentMethod = command.PaymentMethod;
        Status = EBookingStatus.Pending;
    }

    public int Id { get; private set; }
    public int VehicleId { get; private set; }
    public int RenterId { get; private set; }
    public int OwnerId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public decimal PricePerDay { get; private set; }
    public decimal TotalPrice { get; private set; }
    public EBookingStatus Status { get; private set; }
    public EPaymentMethod PaymentMethod { get; private set; }
    public DateTimeOffset? ConfirmedAt { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset? CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Gets the number of rented days (minimum one).</summary>
    public int Days => Math.Max(1, EndDate.DayNumber - StartDate.DayNumber);

    /// <summary>Gets the last night of the rental (the day before the return, or the pick-up day).</summary>
    public DateOnly LastNight => EndDate > StartDate ? EndDate.AddDays(-1) : StartDate;

    /// <summary>Gets a value indicating whether the booking still holds the vehicle (pending or confirmed).</summary>
    public bool IsActive => Status is EBookingStatus.Pending or EBookingStatus.Confirmed;

    /// <summary>
    ///     Checks if this booking blocks the vehicle during the given period.
    /// </summary>
    public bool ConflictsWith(DateOnly startDate, DateOnly endDate) =>
        IsActive && StartDate < endDate && startDate < EndDate;

    /// <summary>
    ///     Gets a value indicating whether the rental already finished (completed, or confirmed and returned).
    /// </summary>
    public bool IsFinishedOn(DateOnly today) =>
        Status == EBookingStatus.Completed || (Status == EBookingStatus.Confirmed && EndDate < today);

    /// <summary>
    ///     Checks if the booking can move to the requested status on the given day.
    /// </summary>
    /// <param name="status">The requested status.</param>
    /// <param name="today">The current business date.</param>
    public bool CanChangeTo(EBookingStatus status, DateOnly today) => status switch
    {
        EBookingStatus.Confirmed or EBookingStatus.Rejected => Status == EBookingStatus.Pending,
        EBookingStatus.Cancelled => IsActive && StartDate >= today,
        EBookingStatus.Completed => Status == EBookingStatus.Confirmed && StartDate <= today,
        _ => false
    };

    /// <summary>
    ///     Moves the booking to the requested status and raises <see cref="BookingStatusChangedEvent" />.
    /// </summary>
    /// <param name="status">The requested status.</param>
    /// <param name="today">The current business date.</param>
    /// <param name="now">The current instant (used as confirmation time).</param>
    /// <exception cref="InvalidOperationException">When the transition is not allowed.</exception>
    public void ChangeStatus(EBookingStatus status, DateOnly today, DateTimeOffset now)
    {
        if (!CanChangeTo(status, today))
            throw new InvalidOperationException($"A {Status} booking cannot change to {status}.");

        Status = status;
        if (status == EBookingStatus.Confirmed) ConfirmedAt = now;
        AddDomainEvent(new BookingStatusChangedEvent(Id, VehicleId, RenterId, OwnerId, Status));
    }
}
