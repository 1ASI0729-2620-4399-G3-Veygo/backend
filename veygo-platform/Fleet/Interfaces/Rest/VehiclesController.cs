using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Fleet.Application.CommandServices;
using Veygo.Platform.Fleet.Application.QueryServices;
using Veygo.Platform.Fleet.Domain.Model.Aggregates;
using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Domain.Model.Queries;
using Veygo.Platform.Fleet.Interfaces.Rest.Resources;
using Veygo.Platform.Fleet.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Application.Model;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Fleet.Interfaces.Rest;

/// <summary>
///     REST controller for vehicles (Fleet bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vehicles")]
public class VehiclesController(
    IVehicleCommandService vehicleCommandService,
    IVehicleQueryService vehicleQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Registers a vehicle")]
    [EndpointDescription("Registers a new vehicle of an owner. The plate must be unique.")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateVehicle([FromBody] CreateVehicleResource resource,
        CancellationToken cancellationToken)
    {
        var command = VehicleCommandFromResourceAssembler.ToCreateCommandFromResource(resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            vehicle => CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id },
                VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets vehicles")]
    [EndpointDescription(
        "Gets all the vehicles. Use ownerId to get the vehicles of an owner and published=true to get the catalog.")]
    [ProducesResponseType(typeof(IEnumerable<VehicleResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetVehicles([FromQuery] int? ownerId, [FromQuery] bool? published,
        CancellationToken cancellationToken)
    {
        IEnumerable<Vehicle> vehicles = ownerId.HasValue
            ? await vehicleQueryService.Handle(new GetVehiclesByOwnerIdQuery(ownerId.Value), cancellationToken)
            : published == true
                ? await vehicleQueryService.Handle(new GetPublishedVehiclesQuery(), cancellationToken)
                : await vehicleQueryService.Handle(new GetAllVehiclesQuery(), cancellationToken);

        if (published.HasValue) vehicles = vehicles.Where(vehicle => vehicle.Published == published.Value);
        return Ok(vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a vehicle by id")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetVehicleById(int id, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleQueryService.Handle(new GetVehicleByIdQuery(id), cancellationToken);
        if (vehicle is null) return NotFound();
        return Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle));
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Updates a vehicle")]
    [EndpointDescription("Replaces the editable data of a vehicle. The owner cannot be changed.")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateVehicle(int id, [FromBody] CreateVehicleResource resource,
        CancellationToken cancellationToken)
    {
        var command = VehicleCommandFromResourceAssembler.ToUpdateCommandFromResource(id, resource);
        var result = await vehicleCommandService.Handle(command, cancellationToken);
        return ToActionResult(result);
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Partially updates a vehicle")]
    [EndpointDescription("Publishes/unpublishes a vehicle (published) and/or updates its rating summary (rating, reviewsCount).")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PatchVehicle(int id, [FromBody] PatchVehicleResource resource,
        CancellationToken cancellationToken)
    {
        Result<Vehicle>? result = null;

        if (resource.Published.HasValue)
        {
            result = await vehicleCommandService.Handle(
                new UpdateVehiclePublicationCommand(id, resource.Published.Value), cancellationToken);
            if (result.IsFailure) return ToActionResult(result);
        }

        if (resource.Rating.HasValue || resource.ReviewsCount.HasValue)
        {
            var current = result?.Value ??
                          await vehicleQueryService.Handle(new GetVehicleByIdQuery(id), cancellationToken);
            if (current is null) return NotFound();
            result = await vehicleCommandService.Handle(
                new UpdateVehicleReputationCommand(id, resource.Rating ?? current.Rating,
                    resource.ReviewsCount ?? current.ReviewsCount), cancellationToken);
        }

        if (result is not null) return ToActionResult(result);

        var vehicle = await vehicleQueryService.Handle(new GetVehicleByIdQuery(id), cancellationToken);
        return vehicle is null ? NotFound() : Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Deletes a vehicle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteVehicle(int id, CancellationToken cancellationToken)
    {
        var result = await vehicleCommandService.Handle(new DeleteVehicleCommand(id), cancellationToken);
        return result.Match<ActionResult>(
            _ => NoContent(),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    private ActionResult ToActionResult(Result<Vehicle> result) =>
        result.Match<ActionResult>(
            vehicle => Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
}
