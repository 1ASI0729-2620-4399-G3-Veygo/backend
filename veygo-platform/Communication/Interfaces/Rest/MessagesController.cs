using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Communication.Application.CommandServices;
using Veygo.Platform.Communication.Application.QueryServices;
using Veygo.Platform.Communication.Domain.Model.Commands;
using Veygo.Platform.Communication.Domain.Model.Queries;
using Veygo.Platform.Communication.Interfaces.Rest.Resources;
using Veygo.Platform.Communication.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Communication.Interfaces.Rest;

/// <summary>
///     REST controller for messages (Communication bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Messages")]
public class MessagesController(
    IMessageCommandService messageCommandService,
    IMessageQueryService messageQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Sends a message")]
    [EndpointDescription("Sends a message in a conversation and updates the conversation's last message.")]
    [ProducesResponseType(typeof(MessageResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateMessage([FromBody] CreateMessageResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateMessageCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await messageCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            message => CreatedAtAction(nameof(GetMessageById), new { id = message.Id },
                MessageResourceFromEntityAssembler.ToResourceFromEntity(message)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets the messages of a conversation")]
    [ProducesResponseType(typeof(IEnumerable<MessageResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetMessagesByConversationId([FromQuery] [BindRequired] int conversationId,
        CancellationToken cancellationToken)
    {
        var messages = await messageQueryService.Handle(new GetMessagesByConversationIdQuery(conversationId),
            cancellationToken);
        return Ok(messages.Select(MessageResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a message by id")]
    [ProducesResponseType(typeof(MessageResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetMessageById(int id, CancellationToken cancellationToken)
    {
        var message = await messageQueryService.Handle(new GetMessageByIdQuery(id), cancellationToken);
        if (message is null) return NotFound();
        return Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(message));
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Marks a message as read")]
    [ProducesResponseType(typeof(MessageResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateMessage(int id, [FromBody] UpdateMessageResource resource,
        CancellationToken cancellationToken)
    {
        if (resource.Read != true)
        {
            var current = await messageQueryService.Handle(new GetMessageByIdQuery(id), cancellationToken);
            return current is null ? NotFound() : Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(current));
        }

        var result = await messageCommandService.Handle(new MarkMessageAsReadCommand(id), cancellationToken);
        return result.Match<ActionResult>(
            message => Ok(MessageResourceFromEntityAssembler.ToResourceFromEntity(message)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }
}
