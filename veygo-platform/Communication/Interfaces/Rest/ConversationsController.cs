using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Communication.Application.CommandServices;
using Veygo.Platform.Communication.Application.QueryServices;
using Veygo.Platform.Communication.Domain.Model.Aggregates;
using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Communication.Domain.Model.Queries;
using Veygo.Platform.Communication.Interfaces.Rest.Resources;
using Veygo.Platform.Communication.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Communication.Interfaces.Rest;

/// <summary>
///     REST controller for conversations (Communication bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Conversations")]
public class ConversationsController(
    IConversationCommandService conversationCommandService,
    IConversationQueryService conversationQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Starts a conversation")]
    [ProducesResponseType(typeof(ConversationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateConversation([FromBody] CreateConversationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateConversationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await conversationCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            conversation => CreatedAtAction(nameof(GetConversationById), new { id = conversation.Id },
                ConversationResourceFromEntityAssembler.ToResourceFromEntity(conversation)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets the conversations of a user")]
    [EndpointDescription("Gets the conversations of a renter (renterId) or of an owner (ownerId), most recent activity first.")]
    [ProducesResponseType(typeof(IEnumerable<ConversationResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetConversations([FromQuery] int? renterId, [FromQuery] int? ownerId,
        CancellationToken cancellationToken)
    {
        IEnumerable<Conversation> conversations;
        if (renterId.HasValue)
            conversations = await conversationQueryService.Handle(new GetConversationsByRenterIdQuery(renterId.Value),
                cancellationToken);
        else if (ownerId.HasValue)
            conversations = await conversationQueryService.Handle(new GetConversationsByOwnerIdQuery(ownerId.Value),
                cancellationToken);
        else
        {
            ModelState.AddModelError(nameof(renterId), "Send renterId or ownerId.");
            return ValidationProblem(ModelState);
        }

        return Ok(conversations.Select(ConversationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a conversation by id")]
    [ProducesResponseType(typeof(ConversationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetConversationById(int id, CancellationToken cancellationToken)
    {
        var conversation = await conversationQueryService.Handle(new GetConversationByIdQuery(id), cancellationToken);
        if (conversation is null) return NotFound();
        return Ok(ConversationResourceFromEntityAssembler.ToResourceFromEntity(conversation));
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Updates the last message preview of a conversation")]
    [EndpointDescription("Sending a message already updates its conversation; this endpoint is kept for clients that update it explicitly.")]
    [ProducesResponseType(typeof(ConversationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateConversation(int id, [FromBody] UpdateConversationResource resource,
        CancellationToken cancellationToken)
    {
        if (resource.LastMessagePreview is null)
        {
            var current = await conversationQueryService.Handle(new GetConversationByIdQuery(id), cancellationToken);
            return current is null
                ? NotFound()
                : Ok(ConversationResourceFromEntityAssembler.ToResourceFromEntity(current));
        }

        var command = new UpdateConversationLastMessageCommand(id, resource.LastMessagePreview,
            resource.LastMessageAt ?? DateTimeOffset.UtcNow);
        var result = await conversationCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            conversation => Ok(ConversationResourceFromEntityAssembler.ToResourceFromEntity(conversation)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }
}
