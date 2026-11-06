using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Reputation.Application.CommandServices;
using Veygo.Platform.Reputation.Application.QueryServices;
using Veygo.Platform.Reputation.Domain.Model.Aggregates;
using Veygo.Platform.Reputation.Domain.Model.Queries;
using Veygo.Platform.Reputation.Interfaces.Rest.Resources;
using Veygo.Platform.Reputation.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Reputation.Interfaces.Rest;

/// <summary>
///     REST controller for reviews (Reputation bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Reviews")]
public class ReviewsController(
    IReviewCommandService reviewCommandService,
    IReviewQueryService reviewQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Reviews a finished rental")]
    [ProducesResponseType(typeof(ReviewResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> CreateReview([FromBody] CreateReviewResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateReviewCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reviewCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            review => CreatedAtAction(nameof(GetReviewById), new { id = review.Id },
                ReviewResourceFromEntityAssembler.ToResourceFromEntity(review)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets reviews")]
    [EndpointDescription(
        "Gets the reviews received by an owner (ownerId), written by a renter (renterId) or of a vehicle (vehicleId), most recent first.")]
    [ProducesResponseType(typeof(IEnumerable<ReviewResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetReviews([FromQuery] int? ownerId, [FromQuery] int? renterId,
        [FromQuery] int? vehicleId, CancellationToken cancellationToken)
    {
        IEnumerable<Review> reviews;
        if (vehicleId.HasValue)
            reviews = await reviewQueryService.Handle(new GetReviewsByVehicleIdQuery(vehicleId.Value), cancellationToken);
        else if (ownerId.HasValue)
            reviews = await reviewQueryService.Handle(new GetReviewsByOwnerIdQuery(ownerId.Value), cancellationToken);
        else if (renterId.HasValue)
            reviews = await reviewQueryService.Handle(new GetReviewsByRenterIdQuery(renterId.Value), cancellationToken);
        else
            reviews = await reviewQueryService.Handle(new GetAllReviewsQuery(), cancellationToken);

        if (ownerId.HasValue) reviews = reviews.Where(review => review.OwnerId == ownerId.Value);
        if (renterId.HasValue) reviews = reviews.Where(review => review.RenterId == renterId.Value);
        return Ok(reviews.Select(ReviewResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a review by id")]
    [ProducesResponseType(typeof(ReviewResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetReviewById(int id, CancellationToken cancellationToken)
    {
        var review = await reviewQueryService.Handle(new GetReviewByIdQuery(id), cancellationToken);
        if (review is null) return NotFound();
        return Ok(ReviewResourceFromEntityAssembler.ToResourceFromEntity(review));
    }
}
