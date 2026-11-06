using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Booking.Application.CommandServices;
using Veygo.Platform.Booking.Application.QueryServices;
using Veygo.Platform.Booking.Domain.Model.Queries;
using Veygo.Platform.Booking.Interfaces.Rest.Resources;
using Veygo.Platform.Booking.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;
using BookingAggregate = Veygo.Platform.Booking.Domain.Model.Aggregates.Booking;

namespace Veygo.Platform.Booking.Interfaces.Rest;

/// <summary>
///     REST controller for bookings (Booking bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Bookings")]
public class BookingsController(
    IBookingCommandService bookingCommandService,
    IBookingQueryService bookingQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Requests a booking")]
    [EndpointDescription(
        "Creates a pending booking. The vehicle must be published, not owned by the renter and available: no pending or confirmed booking overlaps the period and no night is blocked.")]
    [ProducesResponseType(typeof(BookingResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateBooking([FromBody] CreateBookingResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateBookingCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await bookingCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            booking => CreatedAtAction(nameof(GetBookingById), new { id = booking.Id },
                BookingResourceFromEntityAssembler.ToResourceFromEntity(booking)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets bookings")]
    [EndpointDescription(
        "Gets the bookings of a renter (renterId), of an owner (ownerId) or of a vehicle (vehicleId). Without filters it returns all the bookings.")]
    [ProducesResponseType(typeof(IEnumerable<BookingResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetBookings([FromQuery] int? renterId, [FromQuery] int? ownerId,
        [FromQuery] int? vehicleId, CancellationToken cancellationToken)
    {
        IEnumerable<BookingAggregate> bookings;
        if (renterId.HasValue)
            bookings = await bookingQueryService.Handle(new GetBookingsByRenterIdQuery(renterId.Value), cancellationToken);
        else if (ownerId.HasValue)
            bookings = await bookingQueryService.Handle(new GetBookingsByOwnerIdQuery(ownerId.Value), cancellationToken);
        else if (vehicleId.HasValue)
            bookings = await bookingQueryService.Handle(new GetBookingsByVehicleIdQuery(vehicleId.Value),
                cancellationToken);
        else
            bookings = await bookingQueryService.Handle(new GetAllBookingsQuery(), cancellationToken);

        if (vehicleId.HasValue) bookings = bookings.Where(booking => booking.VehicleId == vehicleId.Value);
        if (ownerId.HasValue) bookings = bookings.Where(booking => booking.OwnerId == ownerId.Value);
        return Ok(bookings.Select(BookingResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a booking by id")]
    [ProducesResponseType(typeof(BookingResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBookingById(int id, CancellationToken cancellationToken)
    {
        var booking = await bookingQueryService.Handle(new GetBookingByIdQuery(id), cancellationToken);
        if (booking is null) return NotFound();
        return Ok(BookingResourceFromEntityAssembler.ToResourceFromEntity(booking));
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Changes the status of a booking")]
    [EndpointDescription(
        "Owner confirms or rejects a pending booking; renter or owner cancels an upcoming booking; a confirmed booking can be completed once it started.")]
    [ProducesResponseType(typeof(BookingResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateBookingStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await bookingCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            booking => Ok(BookingResourceFromEntityAssembler.ToResourceFromEntity(booking)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }
}