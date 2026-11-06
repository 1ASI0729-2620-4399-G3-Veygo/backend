using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Engagement.Application.CommandServices;
using Veygo.Platform.Engagement.Application.QueryServices;
using Veygo.Platform.Engagement.Domain.Model.Commands;
using Veygo.Platform.Engagement.Domain.Model.Queries;
using Veygo.Platform.Engagement.Interfaces.Rest.Resources;
using Veygo.Platform.Engagement.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Engagement.Interfaces.Rest;

/// <summary>
///     REST controller for favorite vehicles (Engagement bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Favorites")]
public class FavoritesController(
    IFavoriteCommandService favoriteCommandService,
    IFavoriteQueryService favoriteQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Saves a favorite vehicle")]
    [ProducesResponseType(typeof(FavoriteResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateFavorite([FromBody] CreateFavoriteResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateFavoriteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await favoriteCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            favorite => CreatedAtAction(nameof(GetFavoriteById), new { id = favorite.Id },
                FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets the favorites of a renter")]
    [ProducesResponseType(typeof(IEnumerable<FavoriteResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFavoritesByRenterId([FromQuery] [BindRequired] int renterId,
        CancellationToken cancellationToken)
    {
        var favorites = await favoriteQueryService.Handle(new GetFavoritesByRenterIdQuery(renterId), cancellationToken);
        return Ok(favorites.Select(FavoriteResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a favorite by id")]
    [ProducesResponseType(typeof(FavoriteResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetFavoriteById(int id, CancellationToken cancellationToken)
    {
        var favorite = await favoriteQueryService.Handle(new GetFavoriteByIdQuery(id), cancellationToken);
        if (favorite is null) return NotFound();
        return Ok(FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Removes a favorite")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteFavorite(int id, CancellationToken cancellationToken)
    {
        var result = await favoriteCommandService.Handle(new DeleteFavoriteCommand(id), cancellationToken);
        return result.Match<ActionResult>(
            _ => NoContent(),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }
}
