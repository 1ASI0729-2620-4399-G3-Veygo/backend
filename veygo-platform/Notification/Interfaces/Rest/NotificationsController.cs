using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Veygo.Platform.Notification.Application.CommandServices;
using Veygo.Platform.Notification.Application.QueryServices;
using Veygo.Platform.Notification.Domain.Model.Commands;
using Veygo.Platform.Notification.Domain.Model.Queries;
using Veygo.Platform.Notification.Interfaces.Rest.Resources;
using Veygo.Platform.Notification.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;
using Veygo.Platform.Shared.Resources.Errors;

namespace Veygo.Platform.Notification.Interfaces.Rest;

/// <summary>
///     REST controller for in-app notifications (Notification bounded context).
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Notifications")]
public class NotificationsController(
    INotificationCommandService notificationCommandService,
    INotificationQueryService notificationQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer)
    : ControllerBase
{
    private const int DefaultLimit = 30;
    private const int MaximumLimit = 100;

    [HttpPost]
    [EndpointSummary("Notifies a user")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateNotification([FromBody] CreateNotificationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await notificationCommandService.Handle(command, cancellationToken);
        return result.Match<ActionResult>(
            notification => CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id },
                NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }

    [HttpGet]
    [EndpointSummary("Gets the latest notifications of a user")]
    [EndpointDescription("Most recent first. Use _limit to change how many are returned (default 30, maximum 100).")]
    [ProducesResponseType(typeof(IEnumerable<NotificationResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetNotificationsByUserId([FromQuery] [BindRequired] int userId,
        [FromQuery(Name = "_limit")] int? limit, CancellationToken cancellationToken)
    {
        var take = Math.Clamp(limit ?? DefaultLimit, 1, MaximumLimit);
        var notifications =
            await notificationQueryService.Handle(new GetNotificationsByUserIdQuery(userId, take), cancellationToken);
        return Ok(notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Gets a notification by id")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetNotificationById(int id, CancellationToken cancellationToken)
    {
        var notification = await notificationQueryService.Handle(new GetNotificationByIdQuery(id), cancellationToken);
        if (notification is null) return NotFound();
        return Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification));
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Marks a notification as read")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateNotification(int id, [FromBody] UpdateNotificationResource resource,
        CancellationToken cancellationToken)
    {
        if (resource.Read != true)
        {
            var current = await notificationQueryService.Handle(new GetNotificationByIdQuery(id), cancellationToken);
            return current is null
                ? NotFound()
                : Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(current));
        }

        var result = await notificationCommandService.Handle(new MarkNotificationAsReadCommand(id), cancellationToken);
        return result.Match<ActionResult>(
            notification => Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification)),
            error => ActionResultFromErrorAssembler.ToActionResultFromError(error, this, errorLocalizer));
    }
}
