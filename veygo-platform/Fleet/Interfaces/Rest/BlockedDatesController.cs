using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Fleet.Application.CommandServices;
using Veygo.Platform.Fleet.Application.QueryServices;
using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Domain.Model.Queries;
using Veygo.Platform.Fleet.Interfaces.Rest.Resources;
using Veygo.Platform.Fleet.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Fleet.Interfaces.Rest;

/// <summary>
///     REST controller for the availability calendar of vehicles (blocked days).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Blocked Dates")]
public class BlockedDatesController(
    IBlockedDateCommandService blockedDateCommandService,
    IBlockedDateQueryService blockedDateQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Blocks a day for a vehicle")]
    [ProducesResponseType(typeof(BlockedDateResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateBlockedDate([FromBody] CreateBlockedDateResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateBlockedDateCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await blockedDateCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            blockedDate => CreatedAtAction(nameof(GetBlockedDateById), new { id = blockedDate.Id },
                BlockedDateResourceFromEntityAssembler.ToResourceFromEntity(blockedDate)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets the blocked days of a vehicle")]
    [ProducesResponseType(typeof(IEnumerable<BlockedDateResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBlockedDatesByVehicleId([FromQuery] [BindRequired] int vehicleId,
        CancellationToken cancellationToken)
    {
        var blockedDates =
            await blockedDateQueryService.Handle(new GetBlockedDatesByVehicleIdQuery(vehicleId), cancellationToken);
        return Ok(blockedDates.Select(BlockedDateResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a blocked day by id")]
    [ProducesResponseType(typeof(BlockedDateResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBlockedDateById(int id, CancellationToken cancellationToken)
    {
        var blockedDate = await blockedDateQueryService.Handle(new GetBlockedDateByIdQuery(id), cancellationToken);
        if (blockedDate is null) return NotFound();
        return Ok(BlockedDateResourceFromEntityAssembler.ToResourceFromEntity(blockedDate));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Releases a blocked day")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteBlockedDate(int id, CancellationToken cancellationToken)
    {
        var result = await blockedDateCommandService.Handle(new DeleteBlockedDateCommand(id), cancellationToken);
        return result.Match<ActionResult>(
            _ => NoContent(),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }
}
