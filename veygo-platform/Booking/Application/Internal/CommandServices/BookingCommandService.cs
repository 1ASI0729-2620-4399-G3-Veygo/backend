using Veygo.Platform.Booking.Application.CommandServices;
using Veygo.Platform.Booking.Application.Internal.OutboundServices;
using Veygo.Platform.Booking.Domain.Model.Commands;
using Veygo.Platform.Booking.Domain.Model.Errors;
using Veygo.Platform.Booking.Domain.Repositories;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Domain.Model.ValueObjects;
using Veygo.Platform.Shared.Domain.Repositories;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Application.Internal.CommandServices;

/// <summary>
///     Command service implementation for bookings.
/// </summary>
public class BookingCommandService(
    IBookingRepository bookingRepository,
    ExternalFleetService externalFleetService,
    IUnitOfWork unitOfWork)
    : IBookingCommandService
{
    /// <inheritdoc />
    public async Task<Result<BookingAggregate>> Handle(CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var terms = await externalFleetService.FetchVehicleRentalTermsAsync(command.VehicleId, cancellationToken);
        if (terms is null) return Result<BookingAggregate>.Failure(BookingErrors.VehicleNotFound);
        if (!terms.Published) return Result<BookingAggregate>.Failure(BookingErrors.VehicleNotPublished);
        if (terms.OwnerId == command.RenterId) return Result<BookingAggregate>.Failure(BookingErrors.OwnVehicle);
        if (command.StartDate < BusinessCalendar.Today)
            return Result<BookingAggregate>.Failure(BookingErrors.StartDateInThePast);

        var booking = new BookingAggregate(command, terms);

        if (await bookingRepository.ExistsActiveOverlappingAsync(booking.VehicleId, booking.StartDate,
                booking.EndDate, cancellationToken) ||
            await externalFleetService.HasBlockedNightsAsync(booking.VehicleId, booking.StartDate, booking.LastNight,
                cancellationToken))
            return Result<BookingAggregate>.Failure(BookingErrors.VehicleNotAvailable);

        await bookingRepository.AddAsync(booking, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<BookingAggregate>.Success(booking);
    }

    /// <inheritdoc />
    public async Task<Result<BookingAggregate>> Handle(UpdateBookingStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.FindByIdAsync(command.BookingId, cancellationToken);
        if (booking is null) return Result<BookingAggregate>.Failure(BookingErrors.BookingNotFound);

        var today = BusinessCalendar.Today;
        if (!booking.CanChangeTo(command.Status, today))
            return Result<BookingAggregate>.Failure(BookingErrors.InvalidStatusTransition);

        booking.ChangeStatus(command.Status, today, DateTimeOffset.UtcNow);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<BookingAggregate>.Success(booking);
    }
}
